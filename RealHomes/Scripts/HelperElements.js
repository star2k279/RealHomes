function LocationAutoComplete(obj, objValue) {
    obj.autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                async: false,
                url: '/RHomes/umbraco/surface/AdvSearch/GetLocations?sPrefix=' + obj.val(),
                type: "GET",
                dataType: "json",
                data: { sPrefix: obj.val() },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    try {
                        response($.map(data, function (value) {

                            return {
                                label: value.name,
                                value: value.name,
                                id: value.id
                            };
                        }))

                    } catch (err) { alert('exception occured...'); }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': Something went really wrong !');
                }
            })
        },
        select: function (event, ui) {
            //autocomplete's event that fires after the user select an option from the list
            //ui element has value, label and id properties those were set above when generating the ui-autocomplete suggestion menu
            var selectedId = ui.item.id;
            if (selectedId == "undefined")
            { objValue.val("Enter Location"); }
            else { objValue.val(ui.item.value); } //$("#txtKeyword").val(ui.item.id);
        },
        autoFocus: true,
        change: function (event, ui) {
            if (ui.item == null)
            { objValue.val("Enter Location"); }
            //else { $("#txtKeyword").val(ui.item.id); }
        },
        messages: {
            noResults: function () { }, results: function () { }
        }
    });
}

function AgentAutoComplete(obj, objValue) {
    obj.autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                async: false,
                url: '/RHomes/umbraco/surface/AdvSearch/GetAgentNames?sPrefix=' + obj.val(),
                type: "GET",
                dataType: "json",
                data: { sPrefix: obj.val() },
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    try {
                        response($.map(data, function (value) {

                            return {
                                label: value.name,
                                value: value.name,
                                id: value.id
                            };
                        }))

                    } catch (err) { alert('exception occured...'); }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus + ': Something went really wrong !');
                }
            })
        },
        select: function (event, ui) {
            //autocomplete's event that fires after the user select an option from the list
            //ui element has value, label and id properties those were set above when generating the ui-autocomplete suggestion menu
            var selectedId = ui.item.id;
            //alert(ui.item.id);
            if (selectedId == "undefined")
            { objValue.val("Agent"); }
            else { objValue.val(ui.item.id); } //$("#txtKeywordAgent").val(ui.item.id);

        },
        autoFocus: true,
        change: function (event, ui) {
            if (ui.item == null)
            { objValue.val("Agent"); }
            //else { $("#txtKeyword").val(ui.item.id); }
        },
        messages: {
            noResults: function () { }, results: function () { }
        }
    });

}



