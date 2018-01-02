var agentid = "";
var category = 0;
var service = 0;
var city = 0;

var FirstTime = true;
var pageno = 1;
var criteria = "";

jQuery(document).ready(function () {

    criteria = "";
    AgentAutoComplete($("#txtKeywordAgent"), $("#txtAgentId"));
    //jQuery.noConflict();
    /*
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
            //alert(ui.item.id);
            if (selectedId == "undefined")
            { $("#txtAgentId").val("Agent"); }
            else { $("#txtAgentId").val(ui.item.id); } //$("#txtKeywordAgent").val(ui.item.id);
            
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
    });*/


});
function SearchAgents()
{
    setSearchValues();
    GetSearchResults();
}

function setSearchValues()
{
    if (jQuery("#txtAgentId").val() != "Agent") {
        //set location Id
        agentid = jQuery("#txtAgentId").val();
        
    }
    else
        agentid = "";

    if (jQuery('#ddlCategory option:selected').val() > 0) {
        category = jQuery('#ddlCategory option:selected').text();
    }
    else
        category = "";


    if (jQuery('#ddlService option:selected').val() > 0) {
        service = jQuery('#ddlService option:selected').text();
    }
    else
        service = "";

    if (jQuery('#ddlCity option:selected').val() > 0) {
        city = jQuery('#ddlCity option:selected').text();
    }
    else
        city = "";

    //alert('Agent ID: ' +agentid + ', Category ID: ' +category + ', Service ID: ' + service +', City ID: '+city);
}


function GetSearchResults() {
    
            //call by location and other options
        $.ajax({
            url: '/RHomes/umbraco/surface/AgentCMS/GetAgents',
            async: false,
            type: "GET",
            data: {
                iPageNo: pageno, sSortName: "", sAgentId : agentid, sCategory: category,
                iServiceId: service, sCity: city
                   },
            dataType: "html",
            traditional: true,
            contentType: "application/json",
            error: function (data) {
                alert('Error getting data from ajax call');
            },
            success: function (data, textStatus, jqXHR) {
                //alert(criteria + 'success...' + data);
                console.log(data);
                $("#ResultContainer").empty().append(data);
            }
        });
}






function NextPage(Pg) {
    PageNo = Pg;
    GetSearchResults();
}