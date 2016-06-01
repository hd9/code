using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading;

namespace Infrastructure.Monitoring
{

    public class ServicePointMonitor
    {
        private Action<string> _output;
        private readonly List<ServicePointSummary> connections = new List<ServicePointSummary>();
        
        public class ServicePointSummary
        {
            public int Count { get; set; }
            public int DefaultConnectionLimit { get; set; }
            public List<ServicePointData> ServicePointConnections { get; set; }

            public ServicePointSummary()
            {
                ServicePointConnections = new List<ServicePointData>();
            }
        }

        public class ServicePointData
        {
            public Uri Address { get; set; }
            public int ConnectionLimit { get; set; }
            public int CurrentConnections { get; set; }
            public int ConnectionGroupCount { get; set; }
            public int TotalConnections { get; set; }
            public List<object> ConnectionGroups { get; set; }

            public ServicePointData()
            {
                ConnectionGroups = new List<object>();
            }
        }

        public ServicePointMonitor(Action<string> output = null)
        {
            _output = output;
        }

        public List<ServicePointSummary> GetConnections()
        {
            foreach (var serviceEndpoint in ListServicePoints())
            {
                var spd = new ServicePointSummary
                {
                    Count = serviceEndpoint.Item2,
                    DefaultConnectionLimit = ServicePointManager.DefaultConnectionLimit
                };

                spd.ServicePointConnections.Add(GetServicePointConnections(serviceEndpoint.Item1));
                connections.Add(spd);
            }

            return connections;
        }

        private IEnumerable<Tuple<ServicePoint, int>> ListServicePoints()
        {
            var tableField = typeof(ServicePointManager).GetField("s_ServicePointTable", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            var table = (Hashtable)tableField.GetValue(null);
            var keys = table.Keys.Cast<object>().ToList();

            //Log("ServicePoint count: {0}, DefaultConnectionLimit: {1}", keys.Count, ServicePointManager.DefaultConnectionLimit);
            
            foreach (var key in keys)
            {
                var val = ((WeakReference)table[key]);

                if (val == null)
                {
                    continue;
                }
                var target = val.Target;
                if (target == null)
                {
                    continue;
                }

                yield return new Tuple<ServicePoint, int>(target as ServicePoint, keys.Count);
            }
        }

        //private void Log(string fmt, params object[] args)
        //{
        //    connections.Add(string.Format(fmt, args));
        //}

        private ServicePointData GetServicePointConnections(ServicePoint sp)
        {
            var spType = sp.GetType();
            var privateOrPublicInstanceField = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var connectionGroupField = spType.GetField("m_ConnectionGroupList", privateOrPublicInstanceField);
            var value = (Hashtable)connectionGroupField.GetValue(sp);
            var connectionGroups = value.Keys.Cast<object>().ToList();
            var totalConnections = 0;

            var spd = new ServicePointData
            {
                Address = sp.Address,
                ConnectionLimit = sp.ConnectionLimit,
                CurrentConnections = sp.CurrentConnections
            };

            //Log("ServicePoint: {0} (Connection Limit: {1}, Reported connections: {2})", sp.Address, sp.ConnectionLimit, sp.CurrentConnections);
            
            foreach (var key in connectionGroups)
            {
                var connectionGroup = value[key];
                var groupType = connectionGroup.GetType();
                var listField = groupType.GetField("m_ConnectionList", privateOrPublicInstanceField);
                var listValue = (ArrayList)listField.GetValue(connectionGroup);
                
                //Console.WriteLine("{3} {0}\nConnectionGroup: {1} Count: {2}",sp.Address, key,listValue.Count, DateTime.Now);
                //Log("{0}", key);

                spd.ConnectionGroups.Add(key);

                totalConnections += listValue.Count;
            }

            //Log("ConnectionGroupCount: {0}, Total Connections: {1}", connectionGroups.Count, totalConnections);
            spd.ConnectionGroupCount = connectionGroups.Count;
            spd.TotalConnections = totalConnections;

            return spd;
        }

        public static void Start(TimeSpan interval)
        {
            Start(interval, Console.WriteLine);
        }

        public static void Start(TimeSpan interval, Action<string> output)
        {
            var thread = new Thread(() =>
            {
                while (true)
                {
                    var monitor = new ServicePointMonitor(output);
                    monitor.GetConnections();

                    Thread.Sleep(interval);
                }
            });

            thread.IsBackground = true;
            thread.Start();
        }
    }
}
