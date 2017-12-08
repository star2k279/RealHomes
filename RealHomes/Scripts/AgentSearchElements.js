



jQuery(document).ready(function () {

    criteria = "";
    //jQuery.noConflict();
    $("#txtKeywordAgent").autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                async: false,
                url: '/RHomes/umbraco/surface/AdvSearch/GetAgentNames?sPrefix=' + jQuery('#txtKeywordAgent').val(),
                type: "GET",
                dataType: "json",
                data: { sPrefix: jQuery('#txtKeywordAgent').val() },
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
            { $("#txtAgentId").val("Agent"); }
            else { $("#txtAgentId").val(ui.item.value); } //$("#txtKeywordAgent").val(ui.item.id);
        },
        autoFocus: true,
        change: function (event, ui) {
            if (ui.item == null)
            { $("#txtAgentId").val("Agent"); }
            //else { $("#txtKeyword").val(ui.item.id); }
        },
        messages: {
            noResults: function () { }, results: function () { }
        }
    });


});