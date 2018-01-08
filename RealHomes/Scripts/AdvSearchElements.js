
var country = "uae";
var city;
var community;
var locationName;
var category;
var serviceid;
var propertyType;
var serviceName;
var developmentHold;
var Fixtures = [];
var UnitViews = [];
var minPrice = 0;
var maxPrice = 0;
var minBedRoom = 0;
var maxBedRom = 0;
var currencyUnit = "AED";

var FirstTime = true;
var PageNo = 0;
var searchOption = 0;

var refno;
var criteria = "";
jQuery(document).ready(function () {

    criteria = "";

    LocationAutoComplete($("#txtKeywordLocation"), $("#txtKeyword"));
    //jQuery.noConflict();
    /*
    $("#txtKeywordLocation").autocomplete({
        source: function (request, response) {
            $.ajax({
                cache: false,
                async: false,
                url: '/RHomes/umbraco/surface/AdvSearch/GetLocations?sPrefix=' + jQuery('#txtKeywordLocation').val(),
                type: "GET",
                dataType: "json",
                data: { sPrefix: jQuery('#txtKeywordLocation').val() },
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
            { $("#txtKeyword").val("Enter Location"); }
            else { $("#txtKeyword").val(ui.item.value); } //$("#txtKeyword").val(ui.item.id);
        },
        autoFocus: true,
        change: function (event, ui) {
            if (ui.item == null)
            { $("#txtKeyword").val("Enter Location"); }
            //else { $("#txtKeyword").val(ui.item.id); }
        },
        messages: {
            noResults: function () { }, results: function () { }
        }
    });
*/
    //call multi select plugin on fixture and views drop downs
    $('#ddlFixtures').multiselect({
        maxHeight: 250,
        buttonWidth: 325,

        buttonText: function (options, select) {
            return 'Fixtures and Fittings';
        },
        buttonTitle: function (options, select) {
            var labels = [];
            options.each(function () {
                labels.push($(this).text());
            });
            return labels.join(' - ');
        }
    });
    $('#ddlViews').multiselect({
        buttonWidth: 325,
        maxHeight: 250,

        buttonText: function (options, select) {
            return 'Unit Views';
        },
        includeselectall:true,
        buttonTitle: function (options, select) {
            var labels = [];
            options.each(function () {
                labels.push($(this).text());
            });
            return labels.join(' - ');
        }

    });


    //Load all properties fir the first time. service id parameter should be zero that indicates "all". 
    if (getUrlParameter("location") != "")
    {
        var l = getUrlParameter("location")
        jQuery('#txtKeywordLocation').val(l);
        jQuery('#txtKeyword').val(l);
    }
        SearchProperties();
})

var getUrlParameter = function getUrlParameter(sParam) {
    var sPageURL = decodeURIComponent(window.location.search.substring(1)),
        sURLVariables = sPageURL.split('&'),
        sParameterName,
        i;

    for (i = 0; i < sURLVariables.length; i++) {
        sParameterName = sURLVariables[i].split('=');

        if (sParameterName[0] === sParam) {
            return sParameterName[1] === undefined ? true : sParameterName[1];
        }
    }
};

//This function would be called On the click of Search Buttons "For Sale", "For Rent" and "Short Stays"
function SearchProperties() {
    //Set Search Values according to the search option
    //0= all, 1=Sale, 2=rent, 3=shortstay

    serviceid = jQuery('#ddlService option:selected').val();
    
    if (serviceid == '3') {
        //this will redirect to holiday Homes / short stay area
        return;
    }
    else //if (serviceid == '0' || serviceid == '1' || serviceid == 2) 
    {
        PageNo = 1;
        setSearchValues();

        GetSearchResults();
    }

    
}

//this funcition will set search values on the basis of searchby dropdown list's selected option
function setSearchValues() {
    //ddlSearchBy
    //ddlCategory  ddlPropertyType  ddlFixtures  ddlViews ddldevelopmentLoad
    //ddlminPrice  ddlmaxPrice  ddlminBeds  ddlmaxBeds

    searchOption = jQuery('#ddlSearchBy option:selected').val();
    //iPageno,string sSortName,string sLocationKey
    //if (jQuery('#Page').val() == "")
    //    PageNo = 1;
    //else
    //     PageNo = jQuery('#Page').val();

    switch (searchOption) {
        case "1": //search by location 

            if (jQuery("#txtKeyword").val() != "Enter Location") {
                //set location Id
                locationName = jQuery("#txtKeyword").val();
            }
            else
                locationName = "";

            //set ctegory Id
            if (jQuery('#ddlCategory option:selected').val() > 0) {
                category = jQuery('#ddlCategory option:selected').text();
            }
            else
                category = "";

            //set proerty type Id
            if (jQuery('#ddlPropertyType option:selected').val() > 0)
                propertyType = jQuery('#ddlPropertyType option:selected').text();
            else
                propertyType ="";

            //Set multiple values from fixtures drop down
            Fixtures = [];
            if (jQuery('#ddlFixtures option:selected').length > 0) {
                $('#ddlFixtures :selected').each(function (i, value) {
                    Fixtures[i] = $(value).text();
                    //Fixtures[i] = $(value).text();

                });

            }
            else
                Fixtures = [];
            //set multiple values from views drop down
            UnitViews = [];
            if (jQuery('#ddlViews option:selected').length > 0) {
                $('#ddlViews :selected').each(function (i, value) {
                    UnitViews[i] = $(value).text();
                    //UnitViews[i] = $(value).text();

                });
            }
            else
                UnitViews = [];

            //set development hold status
            if (jQuery('#ddldevHold option:selected').val() > 0)
                developmentHold = jQuery('#ddldevHold option:selected').text();
            else
                developmentHold = "";

            //set min price value
            if (jQuery('#ddlminPrice option:selected').val() > 0)
                minPrice = jQuery('#ddlminPrice option:selected').val();
            else
                minPrice = 0;

            //set max price value 
            if (jQuery('#ddlmaxPrice option:selected').val() > 0)
                maxPrice = jQuery('#ddlmaxPrice option:selected').val();
            else
                maxPrice = 0;

            //set min bed value
            if (jQuery('#ddlminBeds option:selected').val() > 0)
                minBedRoom = jQuery('#ddlminBeds option:selected').text();
            else
                minBedRoom = 0;

            //set max beds value
            if (jQuery('#ddlmaxBeds option:selected').val() > 0)
                maxBedRom = jQuery('#ddlmaxBeds option:selected').text();
            else
                maxBedRom = 0;
            break;
        case "2"://search by reference
            if (jQuery("#txtKeywordRefNo").val() != "Unit Ref# e.g. HA2543" && jQuery("#txtKeywordRefNo").val() != "") {
                //set refno value
                refno = jQuery("#txtKeywordRefNo").val();
            }
            else
                refno = "";
            break;
        case "2":
            break;
        case "3"://search by agent
            if (jQuery("#txtAgentId").val() != "AgentId" && jQuery("#txtAgentId").val() != "") {
                //set refno value
                agentID = jQuery("#txtAgentId").val();
            }
            else
                agentID = "";
            break;
        default:
            break;
    }
}




//httpxdfs://localhost/RHomes/umbraco/surface/Propertysurface/GetProperties
//data: { sCriteria: criteria },
//dffhttp://localhost/RHomes/umbraco/surface/Propertysurface/GetProperties?sCriteria=serviceType=%271%27%20AND%20Category=%271%27

function GetSearchResults() {
    
    //iServiceId, iLocationID, iCategoryId, iTypeId, idevhold, iminbed, imaxbed, iminbath, imaxbath, fixtures, views

    if (searchOption == 1) {

        //call by location and other options
        $.ajax({
            url: '/RHomes/umbraco/surface/PropertyCMS/GetPropertiesBL',
            async: false,
            type: "GET",
            data: {
                iPageno: PageNo, sSortName: "", sLocationKey: "",
                iServiceId: serviceid, sLocationName: locationName, sCategory: category, sType: propertyType,
                sDevhold: developmentHold, iMinbed: minBedRoom, iMaxbed: maxBedRom,
                iMinPrice: minPrice, facilities: Fixtures, iMaxPrice: maxPrice, views: UnitViews
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
    else if (searchOption == 2) {
        //call by refrence
        $.ajax({
            url: '/RHomes/umbraco/surface/PropertyCMS/GetPropertiesBR',
            async: true,
            type: "GET",
            data: { sCriteria: "" },
            dataType: "html",
            contentType: "application/json",
            error: function (data) {
                alert(data);
            },
            success: function (data, textStatus, jqXHR) {
                //alert(criteria + 'success...' + data);
                console.log(data);
                $("#ResultContainer").empty().append(data);
            }
        });
    }
    else if (searchOption == 3) {
        //call by Agent criteria 
        $.ajax({
            url: '/RHomes/umbraco/surface/AgentCMS/GetPropertiesBL',
            async: false,
            type: "GET",
            data: { sCriteria: "" },
            dataType: "html",
            contentType: "application/json",
            error: function (data) {
                alert(data);
            },
            success: function (data, textStatus, jqXHR) {
                //alert(criteria + 'success...' + data);
                console.log(data);
                $("#ResultContainer").empty().append(data);
            }
        });
    }

    return false;

}

function doShortStaySearch() {
    alert('search criteria: ' + criteria);
    //window.location.href = "holidayhomes.RealHomes.com";
}

function NextPage(Pg) {
    PageNo = Pg;
    GetSearchResults();
}