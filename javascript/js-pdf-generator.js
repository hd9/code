<html>
    <head>
        <script src="https://code.jquery.com/jquery-1.12.3.min.js"></script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/0.9.0rc1/jspdf.min.js"></script>
        <script>

            $(document).ready(function(){
                $('#cmd').click(function () {   
                    alert('btnclick!');
                    doc.fromHTML($('#content').html(), 15, 15, {
                        'width': 170,
                            'elementHandlers': specialElementHandlers
                    });
                    doc.save('sample-file.pdf');
                });
            });

            var doc = new jsPDF();
            var specialElementHandlers = {
                '#editor': function (element, renderer) {
                    return true;
                }
            };

        </script>

    </head>
    <body>
        <div id="content">
            <h3>Lorem Ipsum</h3>
            <p>Minus soluta non animi laboriosam voluptas pariatur. Sed eos temporibus culpa dolores iure nulla. Et ut nihil consectetur libero molestiae aut. Reiciendis et impedit officiis veniam quisquam rerum.</p>
            <p>Aperiam ipsam qui ipsam ut quaerat ut reiciendis quis. Ea rerum ducimus rerum nam fugit modi. Qui molestiae nesciunt distinctio aliquid nam. Est culpa tenetur tenetur eos ut nisi quia qui. Recusandae debitis sed voluptatem. Et quia quisquam adipisci numquam dolores est sit ipsum.</p>
            <p>Quaerat sint harum provident nihil totam sunt. Quia dolorem aut iste doloremque et suscipit animi. Distinctio unde doloremque aut et sequi.</p>
                
        </div>
        <div id="editor"></div>
        <button id="cmd">Generate PDF</button>

    </body>
</html>
