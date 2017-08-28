var Api = Api || {};

Api.postify = function (value) {
    var result = {};

    var buildResult = function (object, prefix) {
        for (var key in object) {

            var postKey = isFinite(key)
                ? (prefix != "" ? prefix : "") + "[" + key + "]"
                : (prefix != "" ? prefix + "." : "") + key;

            switch (typeof (object[key])) {
                case "number": case "string": case "boolean":
                    result[postKey] = object[key];
                    break;

                case "object":
                    if (object[key]) {
                        if (object[key].toUTCString) // if date, save value
                            result[postKey] = object[key].toUTCString().replace("UTC", "GMT");
                        else {
                            buildResult(object[key], postKey != "" ? postKey : key);
                        }
                    }
            }
        }
    };

    buildResult(value, "");

    return result;
};

Api.minify = function (json) {

    var tokenizer = /"|(\/\*)|(\*\/)|(\/\/)|\n|\r/g,
        in_string = false,
        in_multiline_comment = false,
        in_singleline_comment = false,
        tmp, tmp2, new_str = [], ns = 0, from = 0, lc, rc
    ;

    tokenizer.lastIndex = 0;

    while (tmp = tokenizer.exec(json)) {
        lc = RegExp.leftContext;
        rc = RegExp.rightContext;
        if (!in_multiline_comment && !in_singleline_comment) {
            tmp2 = lc.substring(from);
            if (!in_string) {
                tmp2 = tmp2.replace(/(\n|\r|\s)*/g, "");
            }
            new_str[ns++] = tmp2;
        }
        from = tokenizer.lastIndex;

        if (tmp[0] == "\"" && !in_multiline_comment && !in_singleline_comment) {
            tmp2 = lc.match(/(\\)*$/);
            if (!in_string || !tmp2 || (tmp2[0].length % 2) == 0) {	// start of string with ", or unescaped " character found to end string
                in_string = !in_string;
            }
            from--; // include " character in next catch
            rc = json.substring(from);
        }
        else if (tmp[0] == "/*" && !in_string && !in_multiline_comment && !in_singleline_comment) {
            in_multiline_comment = true;
        }
        else if (tmp[0] == "*/" && !in_string && in_multiline_comment && !in_singleline_comment) {
            in_multiline_comment = false;
        }
        else if (tmp[0] == "//" && !in_string && !in_multiline_comment && !in_singleline_comment) {
            in_singleline_comment = true;
        }
        else if ((tmp[0] == "\n" || tmp[0] == "\r") && !in_string && !in_multiline_comment && in_singleline_comment) {
            in_singleline_comment = false;
        }
        else if (!in_multiline_comment && !in_singleline_comment && !(/\n|\r|\s/.test(tmp[0]))) {
            new_str[ns++] = tmp[0];
        }
    }
    new_str[ns++] = rc;
    return new_str.join("");
};

Api.clientId = function(unformatted) {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }

    return (unformatted ? s4() + s4() + s4() + s4() + s4() + s4() + s4() + s4() : 'c' + s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4());
};

Api.Command = function (options) {
    var self = this;

    var o = options || {};

    _send = function () {
        if (o.url == null)
            throw "Invalid Url";

        o.data = o.data || {};

        if (o.data != null) {
            o.data["__RequestVerificationToken"] = $('input[name="__RequestVerificationToken"]').val();
            o.data["ClientId"] = o.data["Id"] || Api.Context.clientId;
            o.data["Id"] = o.data["Id"] || Api.Context.id;
        }

        Api.log('[Api.api] Sending ajax request to: "{0}" with data: '.format(o.url), o.data);

        $.ajax({
            url: o.url,
            type: o.type || "POST",
            data: Api.postify(o.data)
        })
        .done(function (data, textStatus, jqXHR) {
            o.success({ data: data, status: textStatus, xhr: jqXHR });
        })
        .fail(function(jqXHR, textStatus, errorThrown) {
            o.error({ xhr: jqXHR, status: textStatus, err: errorThrown });
            console.error("Error issuing ajax call to '{0}': {1}. Please see jqXHR below...".format(o.url, errorThrown));
            console.error(jqXHR);
        });
    }

    self.send = function () {
        if (o.confirm) {
            Api.UI.DialogBox.confirm(o.confirm.msg, function (response) {
                _send();
            }, o.confirm.title);
        } else
            _send();

    }
}
