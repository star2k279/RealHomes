var regionid = 1;
var cityid;
var locationid;
var categoryid;
var propertyTypeid;
var serviceid;
var developmentHold;
var Fixtures = [];
var UnitViews = [];
var minPrice = 0;
var maxPrice = 0;
var minBedRoom = 0;
var maxBedRom = 0;
var currencyUnit="AED";

var FirstTime = true;
var PageNo = 0;
var searchOption = 0;

var refno;
var selectedFromAutocom = false;
var agentID;
var agentDistrict = "";

var html = "";
var placeholder = "";

var criteria = "";
jQuery(document).ready(function () {

    criteria = "";
    //jQuery.noConflict();
    $("#txtKeywordLocation").autocomplete({
                source: function (request, response) {
                $.ajax({
                cache: false,
                async: false,
                url: '/RHomes/umbraco/surface/SearchWidget/GetLocations?sPrefix=' + jQuery('#txtKeywordLocation').val(),
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
                                id:value.id
                            };
                        }))
                      
                    } catch (err) { alert('exception occured...'); }
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert(textStatus +': Something went really wrong !');
                }
                }) 
                },
                select: function (event, ui) {
                    //autocomplete's event that fires after the user select an option from the list
                    //ui element has value, label and id properties those were set above when generating the ui-autocomplete suggestion menu
                    var selectedId = ui.item.id;
                    if (selectedId == "undefined")
                    { $("#txtKeyword").val("Enter Location"); }
                    else { $("#txtKeyword").val(ui.item.id); }
                },
                autoFocus: true,
                change: function (event, ui)
                {
                    if (ui.item == null)
                    { $("#txtKeyword").val("Enter Location"); }
                    //else { $("#txtKeyword").val(ui.item.id); }
                },
                messages: {
                    noResults: function () { }, results: function () { }
                }
    });

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
        buttonWidth:325,
        maxHeight: 250,
        
        buttonText: function (options, select) {
            return 'Unit Views';
        },
        buttonTitle: function (options, select) {
            var labels = [];
            options.each(function () {
                labels.push($(this).text());
            });
            return labels.join(' - ');
        }

    });
    

    //Load all properties fir the first time. service id parameter should be zero that indicates "all". 
    //SearchProperties(1);
})

//This function would be called On the click of Search Buttons "For Sale", "For Rent" and "Short Stays"
function SearchProperties(searchValue)
{
    //Set Search Values according to the search option
    //0= all, 1=Sale, 2=rent, 3=shortstay
    
    if (searchValue == '0' || searchValue == '1' || searchValue=='2')
    {
        serviceid = searchValue;
        PageNo = 1;
        setSearchValues();
        
        GetSearchResults();
    }
    
    else if (searchValue == '3')
    {
        //this will redirect to holiday Homes area
        serviceid = 3;
        setSearchValues();

       
    }
}

//this funcition will set search values on the basis of searchby dropdown list's selected option
function setSearchValues()
{
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
                locationid = jQuery("#txtKeyword").val();
            }
            else
                locationid = 0;

            //set ctegory Id
            if (jQuery('#ddlCategory option:selected').val() > 0) {
                categoryid = jQuery('#ddlCategory option:selected').val();
            }
            else
                categoryid = 0;

            //set proerty type Id
            if (jQuery('#ddlPropertyType option:selected').val() > 0)
                propertyTypeid = jQuery('#ddlPropertyType option:selected').val();
            else
                propertyTypeid = 0;

            //Set multiple values from fixtures drop down
            Fixtures = [];
            if (jQuery('#ddlFixtures option:selected').length > 0)
            {
                $('#ddlFixtures :selected').each(function (i, value)
                {
                    Fixtures[i] = $(value).val();
                    //Fixtures[i] = $(value).text();

                });

            }
            else
                Fixtures = [];
            //set multiple values from views drop down
            UnitViews = [];
            if (jQuery('#ddlViews option:selected').length > 0)
            {
                $('#ddlViews :selected').each(function (i, value)
                {
                    UnitViews[i] = $(value).val();
                    //UnitViews[i] = $(value).text();

                });
            }
            else
                UnitViews = [];

            //set development hold status
            if (jQuery('#ddldevelopmentLoad option:selected').val() > 0)
                developmentHold = jQuery('#ddldevelopmentLoad option:selected').val();
            else
                developmentHold = 0;

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
                minBedRoom = jQuery('#ddlminBeds option:selected').val();
            else
                minBedRoom = 0;

            //set max beds value
            if (jQuery('#ddlmaxBeds option:selected').val() > 0)
                maxBedRom = jQuery('#ddlmaxBeds option:selected').val();
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

function GetSearchResults()
{
    //?sCriteria='+criteria,
    //iServiceId, iLocationID, iCategoryId, iTypeId, idevhold, iminbed, imaxbed, iminbath, imaxbath, fixtures, views

    if (searchOption == 1) 
    {
        
        //call by location and other options
        $.ajax({
            url: '/RHomes/umbraco/surface/Propertysurface/GetPropertiesBL',
            async: false,
            type: "GET",
            data: {
                iPageno: PageNo, sSortName: "", sLocationKey: "",
                iServiceId: serviceid, iLocationID: locationid, iCategoryId: categoryid, iTypeId: propertyTypeid,
                idevhold: developmentHold, iminbed: minBedRoom, imaxbed: maxBedRom,
                iminPrice: minPrice, fixtures: Fixtures, imaxPrice: maxPrice, views: UnitViews
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
    else if (searchOption == 2)
    {
        //call by refrence
        $.ajax({
            url: '/RHomes/umbraco/surface/Propertysurface/GetProperties',
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
    else if (searchOption == 3)
    {
        //call by Agent criteria 
        $.ajax({
            url: '/RHomes/umbraco/surface/Propertysurface/GetProperties',
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
}

function NextPage(Pg)
{
    PageNo = Pg;
    GetSearchResults();
}