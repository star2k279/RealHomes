var countryid = 0;
var categoryid;
var servicetypeid;
var servicePath = "/servicepage.aspx";
var serviceResourcePath = '/local/Services/UnitService.svc/';
var FirstTime = true;
var minPrice = [];
var maxPrice = [];
var BedroomsORSize = [];
var html = "";
var placeholder = "";
var agentDistrict = "";
var cityid;
var objstat;
var obstat2;
var currencyUnit;
var selectedFromAutocom = false;
var agentID;
var perdictiveSearchCachedKeys = [];
var perdictiveSearchCachedData = [];
var hiddenFieldsArr = [];
var allRangesData;
var Headercontrol;
var keyWordsData = [];
var LocationType = [];
var LocationName = []
var ServiceTypeID = []
var CategoryID = []
var UnitTypesArr = ["residential land", "residential building"];
var selectedlocationtype = "";
var MultiValueLimited = 6;
var my_delay = 5000;


function changeplaceholder() {
    var checkChild = $(".bootstrap-tagsinput").children().hasClass("tag");

    if (checkChild == true) {
        $('.tt-input').attr('placeholder', "Add more locations");
    }
    else {
        $('.tt-input').attr('placeholder', "Enter location")
    }
}

$(window).load(function () {
   
    changeplaceholder();
  
});

function Mulitilocation(mystring) {

    var text = mystring;
    var methodArguments = JSON.stringify({ textKeyword: mystring, CategoryID: '0', servicetypeid: '0', CountryCountryID: countryid });



    $.ajax({
        type: "POST",

        url: serviceResourcePath + "getunits",
        contentType: "application/json; charset=utf-8",
        data: methodArguments,
        async: true,
        success: function (data) {



            keyWordsData = [];
            for (var i in data) {
                keyWordsData.push(data[i].LocationName);

            }





        },

        error: function (data) {
            alert(data);
        },
        failure: function () {
            alert('in failure');
        }
    });

}



jQuery(document).ready(function () {

   // var $j = jQuery.noConflict();

    changeplaceholder();

    


    elt = jQuery('#txtKeyword');
    elt.tagsinput({
        maxTags: 6,
        tagClass: function (item) {
            return 'label label-primary';
        },
        itemValue: 'LocationName',
        itemText: 'LocationName',


        typeaheadjs: [
        {

            hint: false,
            highlight: true,
            minLength: 1,
            maxTags: 3,

        },
         {
             name: 'cities',
             displayKey: 'LocationName',
             source: cities.ttAdapter(),
             limit: 10
         }
        ]
    });

    jQuery(".twitter-typeahead").css('display', 'inline');

    //$('input, select').on('change', function (event) {
    //    var $element = $(event.target),
    //      $container = $element.closest('.example');

    //    if (!$element.data('tagsinput'))
    //        return;

    //    var val = $element.val();


    //    StrUnitCode = val;
    //    //alert(StrUnitCode);

    //    if (val === null)
    //        val = "null";

    //    $('code', $('pre.val', $container)).html(($.isArray(val) ? JSON.stringify(val) : "\"" + val.replace('"', '\\"') + "\""));
    //    $('code', $('pre.items', $container)).html(JSON.stringify($element.tagsinput('items')));
    //}).trigger('change');



  

    jQuery('#txtKeyword').on('keydown', function (e) {
        if (e.keyCode === 37 || e.keyCode === 39) { //up or down
            e.preventDefault();
            return false;
        }
    });


    jQuery('ul.dropit li').on('mouseenter', function (event) {
        $target = jQuery(event.currentTarget);
        $sub = $target.children('ul').first();
        $sub.slideToggle();
    }).on('mouseleave', function (event) {
        $target = jQuery(event.currentTarget);
        $target.children('ul').first().slideToggle();
    });

    if (isMobileReq && document.URL.indexOf('search.xhtml') > -1 && jQuery('#allData').val().split('|').length == 21) {

        MobileSiteDefaultValues()
    } else {
        onSelectedIndexChange(jQuery('#ddlUnitTypeForResComCat')[0]);
        DefaultValues();
    }
    //var $j = jQuery.noConflict();
    jQuery("#txtKeywordAgent").autocomplete({
        source: function (request, response) {
            jQuery.ajax({
                type: "POST",
                url: servicePath + "/getagents",
                data: JSON.stringify({
                    textKeyword: jQuery('#txtKeywordAgent').val(),
                    primaryDept: jQuery("#ddlTypeOfService option:selected").text(),
                    areaOFSpec: jQuery('#ddlAreaOfSpeciality').val()
                }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.Agent_name,
                            value: item.Agent_name,
                            agentID: item.Agent_pk
                        }
                    }));
                },
            });
        },
        select: function (a, b) {
            agentID = b.item.agentID;

        }
    }).data("uiAutocomplete")._renderItem = function (ul, item) {
        var term = this.term.split(' ').join('|');
        var re = new RegExp("(" + term + ")", "gi");
        var t = item.label.replace(re, "<b>$1</b>");
        return jQuery("<li></li>").data("item.autocomplete", item).append("<a>" + t + "</a>").appendTo(ul);
    };

    /*Auto complete for Locations
    jQuery('#txtKeywordLocation').on('keydown', function (e) {
        if (e.keyCode === 37 || e.keyCode === 39) { //up or down
            e.preventDefault();
            return false;
        }
    });*/


    $("#txtKeywordLocation").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "/SearchWidget/GetLocations",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({ sPrefix: jQuery('#txtKeywordLocation').val() }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    response($.map(data, function (item) {
                        return {
                            label: item.LocationName,
                            value: item.LocationName
                        };
                    }))

                },  
                error: function() {  
                    alert('something went wrong !');  
                }  
            })
        },
        messages: {
            noResults: function () { }, results: function (){}
        }
    });





    //
    var districtidforstr = "";
    var communityidforstr = "";

    var termTemplate = "<span style='font-weight:bold;'>$1</span>";
    var dataCached = false;

 

    //placeholder change function

  
    
    $('.tt-input').focus(function () {
        //$(this).data('placeholder', $(this).attr('placeholder'))
        //       .attr('placeholder', '');
        changeplaceholder();
    }).blur(function () {
            changeplaceholder();
    });

   


    $('#txtKeyword').on('itemAdded', function (event) {
        // event.item: contains the item
        // event.cancel: set to true to prevent the item getting removed
        changeplaceholder();
    });

    $('#txtKeyword').on('itemRemoved', function (event) {
        // event.item: contains the item
        // event.cancel: set to true to prevent the item getting removed
        changeplaceholder(); 
    });


   

});






function setUnitTypeForKeyword(CategoryID) {
    var obj;

    if (jQuery("#ddlUnitTypeForResComCat").length > 0) {

        if (CategoryID === 2) {
            // var obj = jQuery("#ddlUnitTypeForResComCat option[catid='" + b.item.CategoryID + "']")[0];
            obj = jQuery("#ddlUnitTypeForResComCat option[Value='AllC']")[0];
            jQuery(obj).parent().data("selectBox-selectBoxIt").selectOption(jQuery(obj).val());
        }
        else if (CategoryID === 10) {
            obj = jQuery("#ddlUnitTypeForResComCat option[Value='All']")[0];
            jQuery(obj).parent().data("selectBox-selectBoxIt").selectOption(jQuery(obj).val());
        }
        else {
            obj = jQuery("#ddlUnitTypeForResComCat option[Value='AllR']")[0];
            jQuery(obj).parent().data("selectBox-selectBoxIt").selectOption(jQuery(obj).val());
        }
    }
    else {
        if (CategoryID === 2) {
            // var obj = jQuery("#ddlUnitTypeForResComCat option[catid='" + b.item.CategoryID + "']")[0];
            obj = jQuery("#ddlCategory option[Value='2']")[0];
            jQuery(obj).parent().data("selectBox-selectBoxIt").selectOption(jQuery(obj).val());
        }
        else if (CategoryID === 10) {
            obj = jQuery("#ddlCategory option[Value='10']")[0];
            jQuery(obj).parent().data("selectBox-selectBoxIt").selectOption(jQuery(obj).val());
        }
        else {
            obj = jQuery("#ddlCategory option[Value='1']")[0];
            jQuery(obj).parent().data("selectBox-selectBoxIt").selectOption(jQuery(obj).val());
        }
    }
}
function highlight(data, search, replacedText) {
    var regExp = new RegExp("(" + preg_quote(search) + ")", 'gi');
    return data.replace(regExp, replacedText);
}

function preg_quote(str) {
    return (str + '').replace(/([\\\.\+\*\?\[\^\]\$\(\)\{\}\=\!\<\>\|\:])/g, "\\$1");
}
var minpricefromurl = "";
var maxpricefromurl = "";
var minbedfromurl = "";
var maxbedfromurl = "";
var unitviewsArr = [];
var fixturesArr = [];
var featureArr = [];

function DefaultValues() {

    if (RequestedCountry !== "uae") {

        jQuery('.inputosaurus-container').addClass("inputosaurus-container2");


    }
    if (jQuery('#allData').length > 0 && jQuery('#allData').val() !== "") {


        if (jQuery('#allData').val().split('|').length == 23) {


            jQuery('#txtKeyword').val(jQuery('#allData').val().split('|')[2] == "" ? "" : jQuery('#allData').val().split('|')[2]);


            var collection = jQuery('#txtKeyword').val().split(',');

            if (collection.length == 1) {


                var SingleValueSearch = jQuery('#txtKeyword').val();
                if (collection[0] !== '') {
                    elt.tagsinput('add', { "LocationName": collection[0] });
                }

            }

            else if (collection.length > 1) {

                var LengthKeyWords = collection.length
                for (var i = 0; i < LengthKeyWords; i++) {
                    elt.tagsinput('add', { "LocationName": collection[i] });
                }


            }
            var obj;
           
            minpricefromurl = jQuery('#allData').val().split('|')[5].indexOf("k") !== -1 || jQuery('#allData').val().split('|')[5].indexOf("m") !== -1 ? jQuery('#allData').val().split('|')[5] : jQuery('#allData').val().split('|')[5];
            maxpricefromurl = jQuery('#allData').val().split('|')[6].indexOf("10+") !== -1 ? "10++" : jQuery('#allData').val().split('|')[6].indexOf("k") !== -1 || jQuery('#allData').val().split('|')[6].indexOf("m") !== -1 ? jQuery('#allData').val().split('|')[6] : jQuery('#allData').val().split('|')[6];

            servicetypeid = jQuery('#allData').val().split('|')[0];
            var currCat = jQuery('#allData').val().split('|')[1] == "" ? 0 : jQuery('#allData').val().split('|')[1];
            if (jQuery('#allData').val().split('|')[16] !== "") {
                fixturesArr = jQuery('#allData').val().split('|')[16].split(',');

                SelectCheckBoxes("unit_fixers", fixturesArr);

            }
            if (jQuery('#allData').val().split('|')[18] !== "") {
                unitviewsArr = jQuery('#allData').val().split('|')[18].split(',');
                SelectCheckBoxes("unit_views", unitviewsArr);

            }
            if (jQuery('#allData').val().split('|')[20] !== "") {
                featureArr = jQuery('#allData').val().split('|')[20].split(',');
                SelectCheckBoxes("cblFeaturesList", unitviewsArr);


            }
            if (jQuery("#ddlCategory").length > 0) {

                if (jQuery.inArray(jQuery('#refineValue').val().toLocaleLowerCase(), UnitTypesArr) !== -1) {
                    currCat = 1;
                }

                //jQuery("#ddlCategory").data("selectBox-selectBoxIt").selectOption(currCat);
                setDropDownValue(jQuery("#ddlCategory"), currCat);
                obj = document.getElementById('ddlCategory');
                categoryid = currCat;
            } else {
                obj = document.getElementById('ddlUnitTypeForResComCat');
                categoryid = currCat;
            }

            setServiceControl();
            if (jQuery("#mainContentsPlaceHolder_sidebarsearchcontrol_hdnMainUnitModel").length > 0) {
                jQuery("#mainContentsPlaceHolder_sidebarsearchcontrol_hdnMainUnitModel").data("selectBox-selectBoxIt").selectOption(jQuery('#allData').val().split('|')[20]);
            }
            if (categoryid == 1 || categoryid == 10) {
                minbedfromurl = jQuery('#allData').val().split('|')[7];
                maxbedfromurl = jQuery('#allData').val().split('|')[8];
                //if (minbedfromurl !== maxbedfromurl) {
                //    maxbedfromurl = "All";
                //}

                // maxbedfromurl = maxbedfromurl === '9' ? '9+' : maxbedfromurl;
            } else {
                minbedfromurl = jQuery('#allData').val().split('|')[9];
                maxbedfromurl = jQuery('#allData').val().split('|')[10];

                //if (maxbedfromurl.indexOf('All') == -1) {
                //    maxbedfromurl = TrimString(minbedfromurl) + "-" + TrimString(maxbedfromurl);
                //    checkMaxSize(minbedfromurl);
                //} else {
                //    maxbedfromurl = "All";
                //}

            }
            if (jQuery.inArray(jQuery('#refineValue').val().toLocaleLowerCase(), UnitTypesArr) !== -1) {
                minbedfromurl = jQuery('#allData').val().split('|')[9];
                maxbedfromurl = jQuery('#allData').val().split('|')[10];

                if (maxbedfromurl.indexOf('All') == -1) {
                    maxbedfromurl = TrimString(minbedfromurl) + "-" + TrimString(maxbedfromurl);
                    checkMaxSize(minbedfromurl);
                } else {
                    maxbedfromurl = "All";
                }
            }
            jQuery("#ddlCategorySelectBoxItText").text(jQuery("#ddlCategory :selected").text());
            var tempobj = null;
            if (jQuery('#refineValue') !== null && jQuery('#refineValue').length > 0) {
                refineValue = jQuery('#refineValue').val();
            }
            if (refineValue !== "") {
                if (categoryid == 1 || categoryid == 10) {
                    jQuery("#ddlUnitTypeForResComCat").data("selectBox-selectBoxIt").selectOption(TrimString(refineValue));

                } else if (categoryid == 2) {
                    jQuery("#ddlUnitTypeForResComCat").data("selectBox-selectBoxIt").selectOption(TrimString(refineValue));
                }
            }
            if (maxbedfromurl !== '') {
                setDropDownValue(jQuery("#ddlBedroomsOrSize"), maxbedfromurl);
            }

            if (jQuery("#ddlCategory").length <= 0) {
                onSelectedIndexChange(obj);
            }

            // 
        }
        else {
            if (!isMobileReq) {
                var obj = document.getElementById('ddlUnitTypeForResComCat');
                if (obj !== null) {
                    categoryid = obj.options[obj.selectedIndex].getAttribute('catid');
                    servicetypeid = document.getElementById('ddlServicetype') !== null ? document.getElementById('ddlServicetype').value : jQuery('input[type=radio]:checked').val();
                    onSelectedIndexChange(obj);

                } else {
                    if (jQuery('#allData').val().indexOf("StrUnit") === -1) {
                        obj = document.getElementById('ddlCategory');
                        categoryid = obj.value;
                        servicetypeid = jQuery('input[type=radio]:checked').val();
                        onSelectedIndexChange(obj);

                    }
                }
            }

            if (jQuery('#allData').val().indexOf("StrUnit") !== -1) {
                if (!isMobileReq) {
                    //var obj = document.getElementById('ddlCategory');
                    //obj.Value = "3";



                    categoryid = 3;
                    servicetypeid = 2;
                    var fromDateFormate = jQuery('#allData').val().split('|')[1].split('-');
                    var todateformate = jQuery('#allData').val().split('|')[2].split('-');
                    var dateFrom = fromDateFormate[0] + "-" + fromDateFormate[1] + "-" + fromDateFormate[2];
                    var dateFrom2 = todateformate[0] + "-" + todateformate[1] + "-" + todateformate[2];
                    jQuery('#dateFrom').val(dateFrom);
                    jQuery('#dateFrom2').val(dateFrom2);
                    jQuery("#dateFrom2").datepicker("option", "minDate", dateFrom);
                    jQuery("#dateFrom").datepicker("option", "maxDate", dateFrom2);
                    setUnitTypeForStr(jQuery('#allData').val());
                    //jQuery("#ddlCategory").data("selectBox-selectBoxIt").selectOption(obj.value);
                    jQuery("#ddlUnitType").data("selectBox-selectBoxIt").selectOption(jQuery('#allData').val().split('|')[4]);
                    AjaxCallHeader();
                    setSTRBedRooms();
                    setDropDownValue(jQuery("#ddlBedroomsOrSize"), jQuery('#allData').val().split('|')[5].split('-')[1]);
                    setDropDownValue(jQuery("#ddlminbedsize"), jQuery('#allData').val().split('|')[5].split('-')[0]);
                    //jQuery("#ddlBedroomsStr").data("selectBox-selectBoxIt").selectOption(jQuery('#allData').val().split('|')[5]);
                    jQuery("#main_unitview").hide();
                    jQuery("#str_dates").show();












                }
                else {

                    jQuery('#txtKeywordStr').val(jQuery('#allData').val().split('|')[6] == "" ? "Enter location" : jQuery('#allData').val().split('|')[6]);

                    jQuery(".ui-autocomplete-input").attr("value", jQuery('#txtKeywordStr').val());
                    var collection = jQuery('#txtKeywordStr').val().split(',');




                    //if (collection.length == 1 && collection[0] !== "Enter location") {
                    //    debugger;
                    //    var Newcollection = collection[0].split("-")[2];
                    //    var Delimenator = ',';
                    //    var SingleValueSearch = Newcollection;// jQuery('#txtKeyword').val();
                    //    SingleValueSearch += Delimenator;
                    //    jQuery(".ui-autocomplete-input").attr("value", SingleValueSearch);
                    //    jQuery(".ui-autocomplete-input").keyup();
                    //    jQuery(".ui-autocomplete-input").val("Enter location");
                    //    jQuery(".ui-autocomplete-input").attr("onfocus", "autoHide(this)");
                    //    jQuery(".ui-autocomplete-input").attr("onblur", "autoHide(this)");

                    //}
                    //else if (collection.length > 1) {

                    //    var MultiValueSearch = '';
                    //    var COncatString = [];
                    //    var MultiValues = jQuery('#allData').val().split('|')[6];
                    //    var SplitMultivalueSearch = MultiValues.split(',');
                    //    for (var X = 0; X < SplitMultivalueSearch.length; X++) {
                    //        COncatString[X] = SplitMultivalueSearch[X];
                    //        MultiValueSearch += COncatString[X].split('-')[2] + ',';
                    //    }

                    //    jQuery(".ui-autocomplete-input").attr("value", MultiValueSearch);

                    //    jQuery(".ui-autocomplete-input").keyup();
                    //    jQuery(".ui-autocomplete-input").val("Enter location");
                    //    jQuery(".ui-autocomplete-input").attr("onfocus", "autoHide(this)");
                    //    jQuery(".ui-autocomplete-input").attr("onblur", "autoHide(this)");



                    //}


                    //else {

                    //    jQuery(".ui-autocomplete-input").attr("value", '');
                    //    jQuery(".ui-autocomplete-input").keyup();
                    //    jQuery(".ui-autocomplete-input").val("Enter location");
                    //    jQuery(".ui-autocomplete-input").attr("onfocus", "autoHide(this)");
                    //    jQuery(".ui-autocomplete-input").attr("onblur", "autoHide(this)");

                    //}
                    jQuery('#txtKeywordStr').remove();
                    var obj = document.getElementById('ddlServicetype');
                    obj.value = "3";
                    jQuery("#ddlServicetype").data("selectBox-selectBoxIt").selectOption(obj.value);
                    jQuery('#dateFrom').val(jQuery('#allData').val().split('|')[1]);
                    jQuery('#dateFrom2').val(jQuery('#allData').val().split('|')[2]);
                    onSelectedIndexChange(obj);


                }
            } else if (jQuery('#allData').val().indexOf("refno") !== -1) {
                //var obj = document.getElementById('ddlCategory');
                // obj.value = "5";
                //jQuery("#ddlCategory").data("selectBox-selectBoxIt").selectOption(obj.value);
                jQuery("#txtKeyword").hide();
                jQuery("#txtKeywordRefNoAdvanceOption").show();
                jQuery('#txtKeywordRefNoAdvanceOption').val(jQuery('#allData').val().split('|')[1]);
                onSelectedIndexChange(obj);
                if (jQuery("#SearchTypeChange").length > 0 && jQuery("#SearchTypeChange") !== null) {
                    jQuery("#SearchTypeChange").data("selectBox-selectBoxIt").selectOption("1");
                }


            }

        }
    } else if (false && document.URL.indexOf('agent') !== -1 || document.URL.indexOf('Agent') !== -1) {
        var obj = document.getElementById('ddlCategory');
        obj.value = "4";
        servicetypeid = document.getElementById('ddlServicetype') !== null ? document.getElementById('ddlServicetype').value : jQuery('input[type=radio]:checked').val();
        jQuery("#ddlCategory").data("selectBox-selectBoxIt").selectOption(obj.value);
        onSelectedIndexChange(obj);

    } else if ((false && document.URL.indexOf('short_term_rental') !== -1) && document.URL.indexOf('str') !== -1) {
        var obj = document.getElementById('ddlCategory');
        obj.value = "3";
        servicetypeid = document.getElementById('ddlServicetype') !== null ? document.getElementById('ddlServicetype').value : jQuery('input[type=radio]:checked').val();
        jQuery("#ddlCategory").data("selectBox-selectBoxIt").selectOption(obj.value);
        jQuery('#dateFrom').val('Check-in date');
        jQuery('#dateFrom2').val('Check-out date');
        onSelectedIndexChange(obj);

    } //else if (false && false && document.URL.indexOf('residential') !== -1 || document.URL.indexOf('commercial') !== -1) {
    else if (false) {
        var obj = document.getElementById('ddlUnitTypeForResComCat');
        if (document.URL.indexOf('residential') !== -1) {
            obj.value = "1";
        } else {
            obj.value = "2";
        } if (document.URL.indexOf('lease') !== -1) {
            servicetypeid = "2";
        } else {
            servicetypeid = "1";
        }
        jQuery('input[type=radio]').each(function () {
            if (servicetypeid == jQuery(this).val()) {
                jQuery(this).attr("checked", "checked")
            } else {
                jQuery(this).removeAttr("checked")
            }
        });
        //jQuery("#ddlCategory").data("selectBox-selectBoxIt").selectOption(obj.value);
        onSelectedIndexChange(obj);

    }
    else if (!isMobileReq && document.URL.indexOf("/agents/search.aspx") !== -1) {
        jQuery("#SearchTypeChange").data("selectBox-selectBoxIt").selectOption("3");
    }
    else if (document.URL.indexOf("/agentprofile/agentprofile.aspx?") !== -1) {

        jQuery("#txtKeywordAgent").val(jQuery("#mainContentsPlaceHolder_AgentInfo_AgentName").html());
        if (jQuery("#SearchTypeChange").length > 0) {
            jQuery("#SearchTypeChange").data("selectBox-selectBoxIt").selectOption("3");
        }
    }
    else if (document.URL.indexOf("/agentprofile/agentfeedback.aspx?") !== -1) {
        jQuery("#txtKeywordAgent").val("Search By Agent e.g. Paul Smith");
        if (jQuery("#SearchTypeChange").length > 0) {
            jQuery("#SearchTypeChange").data("selectBox-selectBoxIt").selectOption("3");
        }
    }
    else {

        var obj = document.getElementById('ddlUnitTypeForResComCat');
        if (obj !== null) {
            categoryid = obj.options[obj.selectedIndex].getAttribute('catid');
            servicetypeid = document.getElementById('ddlServicetype') !== null ? document.getElementById('ddlServicetype').value : jQuery('input[type=radio]:checked').val();
            if (servicetypeid === "3") {

                onSelectedIndexChange(jQuery('#ddlServicetype')[0]);
                jQuery("#ddlUnitType").data("selectBox-selectBoxIt").selectOption(jQuery('#hidUnitType').val());
                if (jQuery("#hidBedroomsStr").val() !== "All" && jQuery('#hidBedroomsStr').val() !== null) {
                    jQuery("#ddlBedroomsStr").data("selectBox-selectBoxIt").selectOption(jQuery("#hidBedroomsStr").val())
                }
            } else {
                onSelectedIndexChange(obj);
            }


        } else {
            obj = document.getElementById('ddlCategory');
            categoryid = obj.value;
            servicetypeid = jQuery('input[type=radio]:checked').val();
            onSelectedIndexChange(obj);
        }
    }
    jQuery('.DrawChoice').hide();
    //empty all hidden values
    clearAllHiddenValues();
    minpricefromurl = '';
    maxpricefromurl = '';
    minbedfromurl = '';
    maxbedfromurl = '';
}

function onSelectedIndexChangeSearchBy(obj) {
    // var $j = jQuery.noConflict();

    switch (obj.value) {
        case "1":     // this case for refrecne number 
            jQuery("#Agent").hide();
            jQuery(".inputosaurus-container").hide();
            jQuery(".bootstrap-tagsinput").hide();
            jQuery("#txtKeywordAgent").hide();
            
            
            jQuery("#txtKeywordRefNoAdvanceOption").css("width", "485px");
            jQuery(".newsearch-btn-div").css("width", "auto");
            //jQuery("#btn_sale").addClass("btn_agp");
            jQuery("#txtKeywordRefNoAdvanceOption").fadeIn(400);
            //jQuery("#btn_sale").addClass("btn_agp");
            
            //jQuery("#btn_sale a").text("");
            jQuery("#btn_sale").fadeIn(400);
            jQuery("#btn_rent").fadeIn(400);
            jQuery("#btn_str").fadeIn(400);
            jQuery("#btn_sale a").text("For Sale");
            jQuery("#btn_rent a").text("For Rent");
            jQuery("#btn_str a").text("All");
            jQuery("#btn_str a").attr("id", "btnAll");
			jQuery("#btn_str a").attr("title", "All");
            jQuery("#btn_str a").attr("Value", "5");
            jQuery("#btn_sale a").attr("Value", "1");
            jQuery("#btn_rent a").attr("Value", "2");
           
            jQuery("#btn_sale").removeClass("btn_agp");
            //jQuery("#btn_rent").hide();
            //jQuery("#btn_str").hide();
            jQuery("select#ddlUnitTypeForResComCat").selectBoxIt('disable');
            jQuery("select#ddlminPrice").selectBoxIt('disable');
            jQuery("select#ddlmaxPrice").selectBoxIt('disable');
            jQuery("select#ddlBedroomsOrSize").selectBoxIt('disable');
            jQuery("select#ddlminbedsize").selectBoxIt('disable');
            jQuery("select#ddlcurrncy").selectBoxIt('disable');

            //jQuery(".newsearch-btm-colorline").fadeIn(600)
            //jQuery(".newsearch-btm-txt-3").fadeIn(600)
            //jQuery(".agp_upper_lnk").fadeOut(600)
            jQuery("#normalserach").fadeIn(400);
            jQuery("#adv_lwr_ctrls").slideUp(600);
            $(".srch_uae,.srch_qatar,.srch_jordan,.srch_oman").removeClass("agentSrchCont");
            $(".srch_uae,.srch_qatar,.srch_jordan,.srch_oman").addClass("refSrchCont");
           

            break;
        case "2": // this case for Location 
            jQuery("#Agent").hide();
            jQuery(".bootstrap-tagsinput").hide();
            jQuery("#txtKeywordRefNoAdvanceOption").hide();
            jQuery("#txtKeywordAgent").hide();
            jQuery(".inputosaurus-container").show();
            jQuery(".bootstrap-tagsinput").show();
            jQuery("#btn_sale").removeClass("btn_agp");
            jQuery("#btn_sale a").attr("Value", "1");
            jQuery("#btn_sale a").text("For Sale");
            jQuery("#btn_sale").fadeIn(400);
            jQuery("#btn_rent").fadeIn(400);
            jQuery("#btn_str").fadeIn(400);
            jQuery("#btn_str a").text("Short Stays");
			jQuery("#btn_str a").attr("title", "Short Stays");
            jQuery("#btn_str a").attr("id", "btnStr");
            jQuery("#btn_str a").attr("Value", "3");
            jQuery("select#ddlUnitTypeForResComCat").selectBoxIt('enable');
            jQuery("select#ddlminPrice").selectBoxIt('enable');
            jQuery("select#ddlmaxPrice").selectBoxIt('enable');
            jQuery("select#ddlBedroomsOrSize").selectBoxIt('enable');
            jQuery("select#ddlminbedsize").selectBoxIt('enable');
            jQuery("select#ddlcurrncy").selectBoxIt('enable');
            //jQuery(".newsearch-btm-colorline").fadeIn(600)
            //jQuery(".newsearch-btm-txt-3").fadeIn(600)
            //jQuery(".agp_upper_lnk").fadeOut(600)
            jQuery("#normalserach").fadeIn(400);
            jQuery("#adv_lwr_ctrls").slideDown(600);
            $(".srch_uae,.srch_qatar,.srch_jordan,.srch_oman").removeClass("agentSrchCont");
            $(".srch_uae,.srch_qatar,.srch_jordan,.srch_oman").removeClass("refSrchCont");
            break;
        case "3":

            cityid = document.getElementById('ddlAgentCity').value;
            AjaxCallForAgent();
            jQuery("#normalserach").hide();
            jQuery(".inputosaurus-container").hide();
            jQuery(".bootstrap-tagsinput").hide();
            jQuery(".newsearch-btn-div").css("width", "auto");
            jQuery("#txtKeywordRefNoAdvanceOption").hide();
            jQuery("#txtKeywordAgent").show();
            jQuery("#btn_sale").hide();
            jQuery("#btn_rent").hide();
            jQuery("#btn_str").hide();
            jQuery("#btn_sale").addClass("btn_agp");
            jQuery("#btn_sale a").attr("Value", "4");
            jQuery("#btn_sale a").text("Search");
            jQuery("select#ddlcurrncy").selectBoxIt('disable');
            //jQuery(".newsearch-btm-colorline").fadeOut(600)
            //jQuery(".newsearch-btm-txt-3").fadeOut(600)
            //jQuery(".agp_upper_lnk").fadeIn(600)
            jQuery("#btn_sale").fadeIn(400);
            jQuery("#Agent").fadeIn(400);
            jQuery("#adv_lwr_ctrls").slideUp(600);
            $(".srch_uae,.srch_qatar,.srch_jordan,.srch_oman").addClass("agentSrchCont");
            $(".srch_uae,.srch_qatar,.srch_jordan,.srch_oman").removeClass("refSrchCont");
            break;
        default:
            break;

    }




}

function onSelectedIndexChange(obj) {
    //    jQuery('#hidUnitTypeForResComCat').val('');
    if (typeof obj === "undefined") {
        return;
    }
    if (obj.id !== "ddlServicetype") {
        if (isMobileReq && document.URL.indexOf('search.xhtml') > -1 && jQuery('#allData').val().indexOf("StrUnit") <= -1) {
            changeflag = true;
            onSelectedIndexChangeMobile(obj);
            return true;
        }
    }
    var objectID = obj.id;
    if (jQuery('#ddlUnitTypeForResComCat').length > 0) {
        categoryid = jQuery('#ddlUnitTypeForResComCat option:selected').attr('catid');
        servicetypeid = document.getElementById('ddlServicetype') !== null ? document.getElementById('ddlServicetype').value : jQuery('input[type=radio]:checked').val();
        if (servicetypeid == "3") {
            categoryid = "3";
            objectID = "ddlCategory";
        } else {
            AjaxCallHeader();
        }
    } else {
        categoryid = document.getElementById('ddlCategory').value;
    }
    setServiceControl();
    switch (objectID) {
        case "ddlCategory":

            switch (categoryid) {

                case "10":
                case "1":
                    jQuery('#adv-srch-drop').show();
                    if (jQuery('#advancsearchMinusOrPlus').html() == '(+)') {
                        jQuery('#hiddenRefineDiv').hide();
                    } else {
                        jQuery('#hiddenRefineDiv').show();
                    }
                    jQuery('#txtKeywordRefNo').hide();
                    jQuery('#txtKeywordAgent').hide();
                    jQuery('#unitTypeCom').hide();
                    switch (categoryid) {
                        case "1":
                            jQuery('#unitTypeRes').attr('style', 'float:none;display:block');
                            break;
                        case "10":
                            jQuery('#unitTypeRes').hide();
                            break;
                    }
                    jQuery('#unitTypeRes').css("margin", "0");
                    jQuery('.keyword-capsule').remove();
                    jQuery('#txtKeyword').show();
                    jQuery('#txtKeywordStr').hide();
                    jQuery('#residentcommercial').show();
                    jQuery('#Agent').hide();
                    jQuery('#RefNo').hide();
                    jQuery('#shortTerm').hide();
                    jQuery('#dateFrom').val('Check-in date');
                    jQuery('#dateFrom2').val('Check-out date');
                    jQuery('.DrawChoice').show();
                    AjaxCallHeader();

                    if (jQuery('#unitTypeRes').length > 0) {
                        if (!jQuery('#unitTypeRes').is(':visible')) {
                            jQuery("#ddlUnitTypeForResCategory").data("selectBox-selectBoxIt").selectOption(0);
                        }
                    }
                    break;
                case "2":
                    jQuery('#adv-srch-drop').show();
                    if (jQuery('#advancsearchMinusOrPlus').html() == '(+)') {
                        jQuery('#hiddenRefineDiv').hide();
                    } else {
                        jQuery('#hiddenRefineDiv').show();
                    }
                    jQuery('#txtKeywordRefNo').hide();
                    jQuery('#txtKeywordAgent').hide();
                    jQuery('#unitTypeRes').hide();
                    jQuery('#unitTypeCom').attr('style', 'float:none;display:block');
                    jQuery('#unitTypeCom').css("margin", "0");
                    jQuery('.keyword-capsule').remove();
                    jQuery('#txtKeyword').show();
                    jQuery('#txtKeywordStr').hide();
                    jQuery('#residentcommercial').show();
                    jQuery('#Agent').hide();
                    jQuery('#RefNo').hide();
                    jQuery('#shortTerm').hide();
                    jQuery('#dateFrom').val('Check-in date');
                    jQuery('#dateFrom2').val('Check-out date');
                    AjaxCallHeader();
                    jQuery('.DrawChoice').show();

                    break;
                case "3":

                    jQuery('#adv-srch-drop').show();
                    if (jQuery('#advancsearchMinusOrPlus').html() == '(+)') {
                        jQuery('#hiddenRefineDiv').hide();
                    } else {
                        jQuery('#hiddenRefineDiv').show();
                    }
                    jQuery('#unitTypeCom').hide();
                    jQuery('#unitTypeRes').hide();
                    jQuery('#txtKeywordRefNo').hide();
                    jQuery('#txtKeywordAgent').hide();
                    jQuery('.keyword-capsule').remove();
                    //  jQuery('#txtKeyword').hide();
                    // jQuery('#txtKeywordStr').show();
                    jQuery('#shortTerm').show();
                    jQuery('#residentcommercial').hide();
                    jQuery('#Agent').hide();
                    jQuery('#RefNo').hide();
                    AjaxCallForStr();
                    jQuery('.DrawChoice').hide();
                    jQuery("#ddlUnitType").data("selectBox-selectBoxIt").selectOption(0);
                    setSTRBedRooms();


                    break;
                case "4":
                    jQuery('#adv-srch-drop').hide();
                    jQuery('#unitTypeCom').hide();
                    jQuery('#unitTypeRes').hide();
                    jQuery('#advancsearchMinusOrPlus').html('(+)');
                    jQuery('#hiddenRefineDiv').hide();
                    jQuery('#adv-srch-drop').hide();
                    jQuery('#txtKeyword').hide();
                    jQuery('#txtKeywordRefNo').hide();
                    jQuery('#txtKeyword').hide();
                    jQuery('#txtKeywordStr').hide();
                    jQuery('#txtKeywordAgent').show();
                    jQuery('.keyword-capsule').remove();
                    jQuery('#Agent').show();
                    jQuery('#residentcommercial').hide();
                    jQuery('#RefNo').hide();
                    jQuery('#shortTerm').hide();
                    jQuery('#dateFrom').val('Check-in date');
                    jQuery('#dateFrom2').val('Check-out date');
                    cityid = document.getElementById('ddlAgentCity').value;
                    AjaxCallForAgent();
                    jQuery('.DrawChoice').hide();

                    break;
                case "5":
                    jQuery('#adv-srch-drop').hide();
                    jQuery('#unitTypeCom').hide();
                    jQuery('#unitTypeRes').hide();
                    jQuery('#advancsearchMinusOrPlus').html('(+)');
                    jQuery('#hiddenRefineDiv').hide();
                    jQuery('#adv-srch-drop').hide();
                    jQuery('.keyword-capsule').remove();
                    jQuery('#txtKeyword').hide();
                    jQuery('#txtKeywordAgent').hide();
                    jQuery('#txtKeywordStr').hide();
                    jQuery('#txtKeywordRefNo').show();
                    jQuery('#RefNo').show();
                    jQuery('#residentcommercial').hide();
                    jQuery('#Agent').hide();
                    jQuery('#shortTerm').hide();
                    jQuery('#dateFrom').val('Check-in date');
                    jQuery('#dateFrom2').val('Check-out date');
                    AjaxCallHeader();
                    jQuery('.DrawChoice').hide();

                    break;
            }

            break;
        case "ddlCity":
            cityid = obj.value;
            AjaxCallForStr();


            break;
        case "ddlAgentCity":
            cityid = obj.value;
            AjaxCallForAgent();

            break;
        case "ddlUnitTypeForResComCat":

            categoryid = obj.options[obj.selectedIndex].getAttribute('catid');
            servicetypeid = document.getElementById('ddlServicetype') !== null ? document.getElementById('ddlServicetype').value : jQuery('input[type=radio]:checked').val();
            break;
        case "ddlServicetype":

            jQuery('#adv-srch-drop').show();
            jQuery('#advancsearchMinusOrPlus').html('(+)');
            jQuery('#txtKeywordRefNo').hide();
            jQuery('#txtKeywordAgent').hide();
            jQuery('#unitTypeCom').hide();
            jQuery('#unitTypeRes').show();
            jQuery('#unitTypeRes').css("margin", "0");
            jQuery('.keyword-capsule').remove();

            // jQuery('#txtKeyword').show();
            jQuery('#txtKeywordStr').hide();
            jQuery('#residentcommercial').show();
            jQuery('#Agent').hide();
            jQuery('#RefNo').hide();
            jQuery('#shortTerm').hide();
            jQuery('#dateFrom').val('Check-in date');
            jQuery('#dateFrom2').val('Check-out date');
            jQuery('.DrawChoice').show();
            AjaxCallHeader();
            if (jQuery('#ddlUnitTypeForResComCat').length > 0) {
                jQuery("#ddlUnitTypeForResComCat").data("selectBox-selectBoxIt").selectOption(0);
            }
            break;
        default:
            break;
    }

}

function onSelectedIndexChangeUnitType(obj) {
    if (obj.id == "ddlUnitTypeForResComCat") {
        categoryid = obj.options[obj.selectedIndex].getAttribute('catid');
        servicetypeid = jQuery('#ddlCategory').val();
        AjaxCallHeader();

    } else if (obj.id === "ddlUnitTypeForResCategory") {

        AjaxCallHeader();
        //setBedRoomForPropertyType(jQuery("#ddlBedroomsOrSize"), obj.value);
        if (obj.value == "All Residential" || obj.value === "Residential") {
            jQuery('#hidddlUnitTypeForCategories').val('');
        } else {
            jQuery('#hidddlUnitTypeForCategories').val(document.getElementById('ddlUnitTypeForResCategory').value);
        }
    } else {
        if (obj.value === "All Commercial" || obj.value === "Commercial") {
            jQuery('#hidddlUnitTypeForCategories').val('');
        } else {
            jQuery('#hidddlUnitTypeForCategories').val(document.getElementById('ddlUnitTypeForCommCategory').value);
        }
    }
}

var txtcriteriacompletevalues;
var CacheBedRooms = '';

function setBedRoomForPropertyType(ddlToReset, propertyTypeSelected) {
    if (typeof CacheBedRooms === undefined || TrimString(CacheBedRooms) === '') {
        fillCachedBedroom();
    }
    if (TrimString(CacheBedRooms) !== '') {

        emptyDropdownSelectBox(ddlToReset[0]);
        ddlToReset.data("selectBox-selectBoxIt").add(CacheBedRooms);
        if (propertyTypeSelected.toLocaleLowerCase() === "apartment" || propertyTypeSelected.toLocaleLowerCase() === "all residential" || propertyTypeSelected.toLocaleLowerCase() === "all") {

        } else {
            ddlToReset.data("selectBox-selectBoxIt").remove(1);
        }
    }
}

function txtcriteriaupdation(result) {
    for (var i = 0; i < result.length; i++) {
        result[i] = TrimString(result[i].toString()).replace(/\s+/g, '-');
    }
    txtcriteriacompletevalues = result.toString();

}

function ontxtKeywordchange(obj) {
    placeholder = obj.value;
    jQuery('#hidtxtKeyword').val(obj.value);
}

function criteriaUpdate() {
    switch (categoryid.toString()) {
        case "10":
        case "1":
        case "2":
            if (jQuery("#txtKeyword").length > 0) jQuery('#hidtxtKeyword').val(document.getElementById('txtKeyword').value);
            if (jQuery('#hidUnitTypeForResComCat').length > 0) {
                jQuery('#hidUnitTypeForResComCat').val(document.getElementById('ddlUnitTypeForResComCat').value);
            }
            if (jQuery('#hidddlUnitTypeForCategories').length > 0) {
                jQuery('#hidddlUnitTypeForCategories').val(document.getElementById('hidddlUnitTypeForCategories').value);
            }
            jQuery('#hidCategory').val(categoryid);
            jQuery('#hidServiceID').val(servicetypeid);
            jQuery('#hidminPrice').val(jQuery('#ddlminPrice option:selected').val());
            var maxprice = jQuery('#ddlmaxPrice option:selected').val();
            jQuery('#hidmaxPrice').val(maxprice ? maxprice : '');

            if (document.getElementById('ddlBedroomsOrSize').value !== 'All' && document.getElementById('ddlBedroomsOrSize').value !== '') {
                jQuery('#hidBedroomsOrSize').val(document.getElementById('ddlBedroomsOrSize').value);
            }
            if (jQuery('#ddlminbedsize').length > 0) {
                if (document.getElementById('ddlminbedsize').value !== 'All' && document.getElementById('ddlminbedsize').value !== '') {
                    jQuery('#hidminbedsize').val(document.getElementById('ddlminbedsize').value);
                }
            }
            break;
        case "3":

            jQuery('#hidCategory').val(categoryid);
            jQuery('#hidUnitType').val(document.getElementById('ddlUnitTypeForResComCat').value);
            jQuery('#hidCityStr').val(document.getElementById('ddlCityStr').value);

            var strbeds = isMobileReq ? document.getElementById('ddlBedroomsStr').value + "-" + document.getElementById('ddlBedroomsStr').value : document.getElementById('ddlminbedsize').value + "-" + document.getElementById('ddlBedroomsOrSize').value;
            jQuery('#hidBedroomsStr').val(strbeds);
            jQuery('#hiddateFrom').val(document.getElementById('dateFrom').value);
            jQuery('#hiddateFrom2').val(document.getElementById('dateFrom2').value);
            break;
        case "4":
            jQuery('#hidCategory').val(categoryid);
            jQuery('#hidTypeOfService').val(document.getElementById('ddlTypeOfService').value);
            jQuery('#hidAgentCity').val(document.getElementById('ddlAgentCity').value);
            jQuery('#hidAreaOfSpeciality').val(document.getElementById('ddlAreaOfSpeciality').value);
            jQuery('#hidtxtKeyword').val(document.getElementById('txtKeywordAgent').value);
            jQuery('#hidAgentID').val(agentID);
            break;
        case "5":
            jQuery('#hidCategory').val(categoryid);
            jQuery('#hidtxtKeyword').val(document.getElementById('txtKeywordRefNo').value);
            break;
    }
}
var changeflag = false;

function onSelectedIndexChangeRadio() {
    servicetypeid = jQuery('input[type=radio]:checked').val();
    AjaxCallHeader();

    changeflag = true;
}

function onSelectedIndexDrawMap(obj) {
    servicetypeid = jQuery(obj).find('input[type=radio]:checked').val();
    jQuery('#hidServiceID').val(servicetypeid);
}
function addCommas(string) {
    return string.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
}
function fillddlminPrice(obj) {
    var minDisplayText = isMobileReq ? "Min" : "Price min";
    jQuery('#lblUnitCurrency').text("Price range in " + obj[7]);
    jQuery("#ddlminPrice").data("selectBox-selectBoxIt").remove();
    minPrice = obj[5].split(',');
    //minPrice = sortArr(minPrice);
    html = "<option value='0'> " + minDisplayText + " </option>";
    
    if (countryid === "162") {

        for (var i = 0; i < minPrice.length; i++) {
            if (parseInt(minPrice[i]) > 4 && parseInt(minPrice[i]) !== 6 && parseInt(minPrice[i]) !== 7 && parseInt(minPrice[i]) !== 8 && parseInt(minPrice[i]) !== 9) {
                html += "<option value='" + minPrice[i] + " k'>" + addCommas(minPrice[i]) + "  k</option>";
            }



        }
        for (var i = 0; i < minPrice.length; i++) {
            if (parseInt(minPrice[i]) <= 10) {
                html += "<option value='" + minPrice[i] + " m'>" + addCommas(minPrice[i]) + " " + obj[6] + "</option>";
            }
        }
    }

        //for  Currency 
    else if (countryid === "216") {
        
        for (var i = 0; i < minPrice.length; i++) {

            html += "<option value='" + minPrice[i] + "'>" + addCommas(minPrice[i]) + "  </option>";

        }


    }


   else  {

        for (var i = 0; i < minPrice.length; i++) {
            if (parseInt(minPrice[i]) > 10) {
                html += "<option value='" + minPrice[i] + " k'>" + addCommas(minPrice[i]) + "  k</option>";
            }



        }
        for (var i = 0; i < minPrice.length; i++) {
            if (parseInt(minPrice[i]) <= 10) {
                html += "<option value='" + minPrice[i] + " m'>" + addCommas(minPrice[i]) + " " + obj[6] + "</option>";
            }
        }
    }


  


    jQuery("#ddlminPrice").data("selectBox-selectBoxIt").add(html);
    if (minpricefromurl !== "" && changeflag == false) {
        jQuery("#ddlminPrice").data("selectBox-selectBoxIt").selectOption(TrimString(minpricefromurl));
    } else {
        if (jQuery('#hidminPrice').val() !== null && jQuery("#hidminPrice").val() !== "0" && jQuery('#hidminPrice').val() !== '') {
            jQuery("#ddlminPrice").data("selectBox-selectBoxIt").selectOption(jQuery("#hidminPrice").val())
        } else {
            setDropDownValue(jQuery("#ddlminPrice"), 0);
        }
    }
    html = "";


}

function fillddlmaxPrice(obj) {
    jQuery("#ddlmaxPrice").data("selectBox-selectBoxIt").remove();
    maxPrice = obj[5].split(',');
    var maxDisplayText = isMobileReq ? "Max" : "Price max";
    html = "<option value='Max'>" + maxDisplayText + "</option>";
    if (countryid === "162") {
    
        for (var j = 0; j < maxPrice.length; j++) {
            
            
            if (parseInt(maxPrice[j]) > 4 && parseInt(maxPrice[j]) !== 6 && parseInt(maxPrice[j]) !== 7 && parseInt(maxPrice[j]) !== 8 && parseInt(maxPrice[j]) !== 9) {
                html += "<option value='" + maxPrice[j] + " k'>" + addCommas(maxPrice[j]) + "  k</option>";
            }


        }
        for (var j = 0; j < maxPrice.length; j++) {


            if (parseInt(maxPrice[j]) < 10) {
                html += "<option value='" + maxPrice[j] + " m'>" + addCommas(maxPrice[j]) + " " + obj[6] + "</option>";
            }
            else if (parseInt(maxPrice[j]) == 10)
                html += "<option value='" + maxPrice[j] + "++'>" + addCommas(maxPrice[j]) + "+ " + obj[6] + "</option>";
        }

    }


    else if (countryid === "216") {
        for (var j = 0; j < maxPrice.length - 1; j++) {

            html += "<option value='" + maxPrice[j] + "'>" + addCommas(maxPrice[j]) + " </option>";

        }

        for (var j = 0; j < maxPrice.length; j++) {

            if ((maxPrice[j]) == '5 m')
                html += "<option value='" + maxPrice[j] + "++'>" + addCommas(maxPrice[j]) + "+ </option>";
        }
    }


   else {
        for (var j = 0; j < maxPrice.length; j++) {
            if (parseInt(maxPrice[j]) > 10) {
                html += "<option value='" + maxPrice[j] + " k'>" + addCommas(maxPrice[j]) + "  k</option>";
            }
        }
        for (var j = 0; j < maxPrice.length; j++) {
            if (parseInt(maxPrice[j]) < 10) {
                html += "<option value='" + maxPrice[j] + " m'>" + addCommas(maxPrice[j]) + " " + obj[6] + "</option>";
            }
            else if (parseInt(maxPrice[j]) == 10)
                html += "<option value='" + maxPrice[j] + "++'>" + addCommas(maxPrice[j]) + "+ " + obj[6] + "</option>";
        }

    }



    jQuery("#ddlmaxPrice").data("selectBox-selectBoxIt").add(html);
    if (maxpricefromurl !== "" && changeflag == false) {
        jQuery("#ddlmaxPrice").data("selectBox-selectBoxIt").selectOption(TrimString(maxpricefromurl));
    } else {
        if (jQuery("#hidmaxPrice").val() !== "Max" && jQuery('#hidmaxPrice').val() !== null && jQuery('#hidmaxPrice').val() !== '') {
            jQuery("#ddlmaxPrice").data("selectBox-selectBoxIt").selectOption(jQuery("#hidmaxPrice").val())
        } else {
            setDropDownValue(jQuery("#ddlmaxPrice"), 'Max');
        }
    }
    html = "";
}

function fillddlBedroomsorSize(obj, SizeRange) {

    jQuery("#ddlBedroomsOrSize").data("selectBox-selectBoxIt").remove();
    jQuery("#ddlminbedsize").length > 0 ? jQuery("#ddlminbedsize").data("selectBox-selectBoxIt").remove() : jQuery("#ddlBedroomsOrSize").data("selectBox-selectBoxIt").remove();


    var k;
    var txtbedsize;
    var txtminbedsize;
    var minbedsizehtml;
    var bedrooms;
    var IsSizeData = false;
    var categoryddlList = jQuery('#ddlUnitTypeForResCategory').length > 0 ? jQuery('#ddlUnitTypeForResCategory') : jQuery('#ddlUnitTypeForResComCat');
    if ((categoryid.toString() == "1" || categoryid.toString() == "10") && jQuery.inArray(categoryddlList.val().toLocaleLowerCase(), UnitTypesArr) === -1) {
        jQuery('#lblBedOrSize').text("Bedrooms");
        if (!isMobileReq) {
            txtbedsize = "Max bedrooms";
            txtminbedsize = "Min bedrooms";
        }
        else {
            txtbedsize = "All";
            txtminbedsize = "All";
        }

        BedroomsORSize = obj[5].split(',');
    } else if (categoryid == 2 || jQuery.inArray(categoryddlList.val().toLocaleLowerCase(), UnitTypesArr) !== -1) {
        jQuery('#lblBedOrSize').text("Size (" + SizeRange[6] + ")");
        BedroomsORSize = SizeRange[5].split(',');
        IsSizeData = true;
        if (!isMobileReq) {
            txtbedsize = "Max size"
            txtminbedsize = "Min size";
        }
        else {
            txtbedsize = "All";
            txtminbedsize = "All";
        }
    }

    if (!IsSizeData) {
        for (k = 0; k < BedroomsORSize.length; k++) {
            if (k == 0) {
                html += "<option value='All' selected='selected'>" + txtbedsize + "</option>";
                //injecting minimum maximum bedrange or SizeRange
                minbedsizehtml += "<option value='All' selected='selected'>" + txtminbedsize + "</option>";

            }
            if (categoryddlList.val().toLocaleLowerCase() !== 'villa' && TrimString(BedroomsORSize[k]) == "0" && (categoryid == 1 || categoryid == 10)) {

                //injecting studio or beds
                html += "<option value='" + TrimString(BedroomsORSize[k]) + "'>Studio</option>";


            } else {
                if (TrimString(BedroomsORSize[k]) !== "0") {
                    html += "<option value='" + TrimString(BedroomsORSize[k]) + "'>" + BedroomsORSize[k] + "</option>";
                    minbedsizehtml += "<option value='" + TrimString(BedroomsORSize[k]) + "'>" + BedroomsORSize[k] + "</option>";
                }
            }
        }
    }
    else {
        for (k = 0; k < BedroomsORSize.length; k++) {
            if (k == 0) {
                html += "<option value='All' selected='selected'>" + txtbedsize + "</option>";

                //injecting minimum maximum bedrange or SizeRange
                minbedsizehtml += "<option value='All' selected='selected'>" + txtminbedsize + "</option>";
            }
            var minSize = typeof BedroomsORSize[k].split('-')[0] !== "undefined" ? BedroomsORSize[k].split('-')[0] : BedroomsORSize[k];
            var maxSize = typeof BedroomsORSize[k].split('-')[1] !== "undefined" ? BedroomsORSize[k].split('-')[1] : BedroomsORSize[k];
            html += "<option value='" + TrimString(maxSize) + "'>" + maxSize + "</option>";
            minbedsizehtml += "<option value='" + TrimString(minSize) + "'>" + minSize + "</option>";

        }
    }
    jQuery("#ddlBedroomsOrSize").data("selectBox-selectBoxIt").add(html);
    jQuery("#ddlminbedsize").length > 0 ? jQuery("#ddlminbedsize").data("selectBox-selectBoxIt").add(minbedsizehtml) : "";
    if (jQuery("#ddlminbedsize").length > 0) {
        if (minbedfromurl !== "") {

            setDropDownValue(jQuery("#ddlminbedsize"), minbedfromurl);
        }
    }
    if (maxbedfromurl !== "") {
        setDropDownValue(jQuery("#ddlBedroomsOrSize"), maxbedfromurl);
    } else {
        jQuery("#ddlBedroomsOrSize").data("selectBox-selectBoxIt").selectOption(txtbedsize);
        if (jQuery("#hidBedroomsOrSize").val() !== "All" && jQuery('#hidBedroomsOrSize').val() !== null && jQuery("#hidBedroomsOrSize").val() !== "") {
            jQuery("#ddlBedroomsOrSize").data("selectBox-selectBoxIt").selectOption(jQuery("#hidBedroomsOrSize").val())

        }
        else {

            setDropDownValue(jQuery("#ddlBedroomsOrSize"), "All");
        }

        if (jQuery("#ddlminbedsize").length > 0 && jQuery("#hidminbedsize").val() !== "All" && jQuery('#hidminbedsize').val() !== null && jQuery("#hidminbedsize").val() !== "") {
            jQuery("#ddlminbedsize").data("selectBox-selectBoxIt").selectOption(jQuery("#hidminbedsize").val())
        }
        else {

            setDropDownValue(jQuery("#ddlBedroomsOrSize"), "All");
        }

    }
    html = "";

}

function AjaxCallHeader(firstLoad) {

    if (typeof allRangesData === "undefined") {
        allRangesData = rangesJSON;
    }

    if (typeof allRangesData !== "undefined" && allRangesData && allRangesData.length > 0) {

        var filterCategoryId = (categoryid === "3" ? "1" : categoryid.toString());
        var filterServiceId = (servicetypeid === "3" ? "2" : servicetypeid.toString());
        var selectedRanges = filterArray(allRangesData, function (rData) {
            return rData.CategoryId.toString() === filterCategoryId && rData.ServiceTypeId.toString() === filterServiceId;
        });
        var priceRanges = filterArray(selectedRanges, function (priceData) { return priceData.Text.toLocaleLowerCase() === "price"; });

        var bedorSizeRange = filterArray(selectedRanges, function (bedData) { return filterCategoryId === "2" ? bedData.Text.toLocaleLowerCase() === 'size' : bedData.Text.toLocaleLowerCase() === 'bedrooms'; });
        var SizeRange = filterArray(allRangesData, function (rData) {
            return rData.CategoryId.toString() === "2" && rData.ServiceTypeId.toString() === filterServiceId && rData.Text.toLocaleLowerCase() === 'size';
        });
      
        if (priceRanges && priceRanges.length > 0) {
            var priceRange = convertIntoRangeArray(priceRanges[0]);
            fillddlminPrice(priceRange);
            fillddlmaxPrice(priceRange);


            if (bedorSizeRange && bedorSizeRange.length > 0) {
                var bedroomOrSizeRange = convertIntoRangeArray(bedorSizeRange[0]);
                fillddlBedroomsorSize(bedroomOrSizeRange, convertIntoRangeArray(SizeRange[0]));
            }
        }
    }
}

function AjaxCallForAgent() {
    servicePath = "/servicepage.aspx";
    jQuery.ajax({
        type: "POST",
        url: servicePath + "/getdistrict",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            cityid: cityid
        }),
        datatype: "json",
        processData: true,
        success: function (result) {
            document.getElementById('ddlAreaOfSpeciality').innerHTML = "";
            if (result.d.length > 0) {
                for (var i = 0; i < result.d.length; i++) {
                    var value = result.d[i].District_pk;
                    var text = result.d[i].District_name;
                    html += "<option value='" + value + "'>" + text + "</option>";
                }
                jQuery("#ddlAreaOfSpeciality").data("selectBox-selectBoxIt").add(html);
                html = "";
            }
        },
        error: function () { },
        failure: function () { },
        complete: function () { }
    });
}

function AjaxCallForStr() {
    var defaultSelection = "Dubai";
    jQuery.ajax({
        type: "POST",
        url: servicePath + "/getcities",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
            CountryId: countryid
        }),
        datatype: "json",
        processData: true,
        success: function (result) {
            document.getElementById('ddlCityStr').innerHTML = "";
            if (result.d.length > 0) {
                for (var i = 0; i < result.d.length; i++) {
                    var value = result.d[i].City_pk;
                    var text = result.d[i].City_name;
                    if (defaultSelection == text) html += "<option selected value='" + value + "'>" + text + "</option>";
                    else html += "<option value='" + value + "'>" + text + "</option>";
                }
                jQuery("#ddlCityStr").data("selectBox-selectBoxIt").add(html);
                document.getElementById('ddlCityStr').selectedIndex.value = 54788;
                html = "";
            }

        },
        error: function () { },
        failure: function () { },
        complete: function () { }
    });
}

function get_answersSrt(v, cont) {
    jQuery('#hidCityStr').val(document.getElementById('ddlCityStr').value);
    jQuery.get('/AutoSuggest/revamp-suggest.aspx?cityid=' + jQuery('#hidCityStr').val() + '&isStr=t&q=' + jQuery(v)[0].term, function (suggests) {
        cont($.map(suggests, function (item) {
            return {
                label: item.inputCol + "-" + item.Type,
                value: item.inputCol,
                Type: item.Type,
                mainId: item.mainId
            }
        }));
    }, 'json')
}

function get_answers(v, cont) {
    jQuery.get('/AutoSuggest/revamp-suggest.aspx?CountryId=' + countryid + '&IsInternational=0&serviceTypeId=' + jQuery("#hidServiceID").val() + '&categoryIdMajor=' + jQuery("#hidCategory").val() + '&minPrice=0&maxPrice=20000000&minBeds=0&maxBeds=9&minSize=0&maxSize=200000000&q=' + jQuery(v)[0].term, function (suggests) {
        cont($.map(suggests, function (item) {
            return {
                label: item.inputCol + "-" + item.Type,
                value: item.inputCol
            }
        }));
    }, 'json')
}

function MobileSiteDefaultValues() {

    if (jQuery('#allData').length > 0) {
        if (jQuery('#allData').val().split('|').length == 22) {
            jQuery('#txtKeyword').val(jQuery('#allData').val().split('|')[2]);
            var obj;
            minpricefromurl = jQuery('#allData').val().split('|')[5];
            maxpricefromurl = jQuery('#allData').val().split('|')[6];
            obj = document.getElementById('ddlUnitTypeForResComCat');
            servicetypeid = jQuery('#allData').val().split('|')[0];
            jQuery('#ddlServicetype').find('option').removeAttr('selected');
            jQuery('#ddlServicetype').find('option[value="' + servicetypeid + '"]').attr("selected", true);
            jQuery("#ddlServicetypeSelectBoxItText").text(jQuery('#ddlServicetype').find('option[value="' + servicetypeid + '"]').text());
            categoryid = jQuery('#allData').val().split('|')[1];
            jQuery('#ddlUnitTypeForResComCat').find('option').removeAttr('selected');
            jQuery('#ddlUnitTypeForResComCat').find('option[catid="' + categoryid + '"]').attr("selected", true);
            if (categoryid == 1 || categoryid == 10) {
                minbedfromurl = jQuery('#allData').val().split('|')[7];
                maxbedfromurl = jQuery('#allData').val().split('|')[8];
                if (minbedfromurl !== maxbedfromurl) {
                    maxbedfromurl = "All";
                }
            } else {
                minbedfromurl = jQuery('#allData').val().split('|')[9];
                maxbedfromurl = jQuery('#allData').val().split('|')[10];
                if (countryid !== 35) {
                    maxbedfromurl = maxbedfromurl;
                } else {
                    if (maxbedfromurl.indexOf('All') == -1) {
                        maxbedfromurl = TrimString(minbedfromurl) + "-" + TrimString(maxbedfromurl);
                    } else {
                        maxbedfromurl = "All";
                    }
                }
            }
            var tempobj = null;
            if (refineValue !== "") {
                if (categoryid == 1) {
                    jQuery("#ddlUnitTypeForResComCatSelectBoxItText").text(TrimString(refineValue));
                } else if (categoryid == 2) {
                    jQuery("#ddlUnitTypeForResComCatSelectBoxItText").text(TrimString(refineValue));
                }
            }
            onSelectedIndexChangeMobile(obj);

        }
    }
}

function onSelectedIndexChangeMobile(obj) {

    servicetypeid = jQuery('#ddlServicetype').val();
    categoryid = jQuery('#ddlUnitTypeForResComCat :selected').attr('catid');
    if (servicetypeid == "3") {
        categoryid = "3";
        objectID = "ddlCategory";
    } else {
        AjaxCallHeader();
    }
}
function emptyEnquiryFormDropdown(obj) {
    jQuery(obj).data("selectBox-selectBoxIt").remove();
}


function cachedBedroomsData(BedroomsORSize) {
    if ((typeof CacheBedRooms === undefined || TrimString(CacheBedRooms) === '') && BedroomsORSize.length > 0) {
        for (k = 0; k < BedroomsORSize.length; k++) {
            if (k == 0) {
                CacheBedRooms += "<option value='All'>All</option>";
            }
            if (TrimString(BedroomsORSize[k]) == "0") {
                CacheBedRooms += "<option value='" + TrimString(BedroomsORSize[k]) + "'>Studio</option>";
            } else {
                CacheBedRooms += "<option value='" + TrimString(BedroomsORSize[k]) + "'>" + BedroomsORSize[k] + "</option>";
            }
        }
    }
}

function emptyDropdownSelectBox(obj) {
    jQuery(obj).data("selectBox-selectBoxIt").remove();
}
function OnChangeSTR() {
    setBedRoomForPropertyType(jQuery("#ddlBedroomsStr"), unitTypeSelected);
}
function setSTRBedRooms() {
    var unitTypeSelected = jQuery('#ddlUnitType option:selected').text();
    setBedRoomForPropertyType(jQuery("#ddlBedroomsStr"), unitTypeSelected);
}
function convertIntoArray(objectToConvert) {
    return Object.keys(objectToConvert).map(function (k) { return objectToConvert[k] });
}
function filterArray(arrayToFilter, criteria) {
    return jQuery.grep(arrayToFilter, criteria);
}
function convertIntoRangeArray(objectToConvert) {
    var rangeArray = [];
    rangeArray.push(objectToConvert.CountryId);
    rangeArray.push(objectToConvert.CategoryId);
    rangeArray.push(objectToConvert.ServiceTypeId);
    rangeArray.push(objectToConvert.Text);
    rangeArray.push(objectToConvert.Multiplier);
    rangeArray.push(objectToConvert.SliderValues);
    rangeArray.push(objectToConvert.UnitValue);
    rangeArray.push(objectToConvert.Currency);
    return rangeArray;
}
function fillCachedBedroom() {
    if (!allRangesData) {
        allRangesData = rangesJSON;
    }
    var bedRangeData = filterArray(allRangesData, function (bedData) { return bedData.Text.toLocaleLowerCase() === 'bedrooms'; });
    cachedBedroomsData(bedRangeData[0].SliderValues.split(','));
}
function setServiceControl() {

    jQuery('input[id^="rdoServiceID"][type=radio]').each(function () {
        if (servicetypeid == jQuery(this).val()) {
            jQuery(this).attr("checked", "checked")
        } else {
            jQuery(this).removeAttr("checked")
        }
    });
}
function clearAllHiddenValues() {
    hiddenFieldsArr = ["hidCategory", "hidUnitTypeForResComCat", "hidtxtKeyword", "hidServiceID", "hidminPrice", "hidmaxPrice", "hidBedroomsOrSize", "hidminbedsize", "hidUnitType", "hidCityStr", "hidDistrictStr", "hidCommunityStr", "hidBedroomsStr", "hiddateFrom", "hiddateFrom2", "hidAgentID", "hidTypeOfService", "hidAgentCity", "hidAreaOfSpeciality", "hdncurrency"];
    for (var i = 0; i < hiddenFieldsArr.length; i++) {
        jQuery("input[id=" + hiddenFieldsArr[i] + "]").val('');
    }
}
function setDropDownValue(jQueryDropDownControl, valueToSet) {
    jQueryDropDownControl.data("selectBox-selectBoxIt").selectOption(valueToSet);
    jQueryDropDownControl.val(valueToSet);
}

function Loadkeyworddata() {
    var keywords = "";
    var arrLength = MultiKeywordContainer.length;
    if (arrLength > 0) {
        for (var i = 0; i < arrLength; i++) {
            if (i > 0) {
                keywords += ",";
            }
            keywords += MultiKeywordContainer[i];

        }
        jQuery('#hidtxtKeyword').val(keywords);
    }
    else {
        jQuery('#hidtxtKeyword').val('Enter location');
    }

}
function setServicetype(obj) {
    if (obj !== null && obj !== undefined) {
        jQuery(obj).addClass('active').siblings().removeClass('active');
        var serviceobj = jQuery(obj).parent();
        newserviceID = jQuery(obj).attr("servicevalue");
        onSelectedIndexChange(document.getElementById('ddlServicetype'));
        resetMultikeywords();
    }

}
function resetMultikeywords() {
    jQuery("#multi-search li").each(function () {
        if (this.id === "txt-li") {
            return;
        }
        else {
            this.remove();
        }
    });
    MultiKeywordContainer = new Array();
    jQuery('#txtKeyword').css('width', 'auto').val(defaultkeyword);
    jQuery('#txtKeywordStr').css('width', 'auto').val(defaultkeyword);

}
function GetTheDate(dateOf) {
    var dateToday = new Date();
    var dd = dateToday.getDate();
    var mm;
    switch (dateOf) {
        case "aftermonth":
            mm = dateToday.getMonth() + 2;
            break;
        case "today":
            mm = dateToday.getMonth() + 1;
            break;
        default:
            break;

    }

    var yyyy = dateToday.getFullYear();

    if (dd < 10) {
        dd = '0' + dd
    }

    if (mm < 10) {
        mm = '0' + mm
    }


    dateToday = dd + '-' + mm + '-' + yyyy;
    return dateToday;
}
function SetKeyWordData(multiKeywords) {
    var multiKeywordsArr = multiKeywords.split(',');
    var multiKeywordshtml = "";
    fromDefault = true;
    if (multiKeywordsArr !== null && multiKeywordsArr.length > 0) {
        for (var i = 0; i < multiKeywordsArr.length; i++) {

            if (multiKeywordsArr[i].toLocaleLowerCase() !== "enter location" && multiKeywordsArr[i] !== "undefine") {

                GetMultiSearchKeyword(multiKeywordsArr[i], jQuery("#txtKeyword"));
                jQuery("#searchkeyword-div").animate({ scrollTop: 0 }, "fast");
                jQuery("#txtKeyword").val(defaultkeyword);

            }

        }
    }
}
function GetMultiSearchKeyword(Keyword, objtxtfield) {

    if (!isAllreadyExist(Keyword)) {
        var MultiSerarchKeyword = "<li class='auto-mulitple-capsole' ><div class='keytxt'>" + Keyword + "</div>";
        var btnDelete = "<span class='close-multi' onclick='deleteMultiCapsole(this);'>&#10006;</span>";
        MultiSerarchKeyword += btnDelete + "</li>";
        objtxtfield.val('');
        MultiSerarchKeywordCounter = MultiSerarchKeywordCounter + 1;

        if (MultiSerarchKeywordCounter >= 6) {
            objtxtfield.hide();

        }
        MultiKeywordContainer.push(Keyword);
        MultiSerarchKeyword;
        jQuery("#txt-li").before(MultiSerarchKeyword);
        //objtxtfield.focus();
        if (!fromDefault) {
            var scrolledContainer = jQuery(".main-srch-2").length !== 0 ? jQuery(".main-srch-2") : jQuery("#searchkeyword-div");
            var height = scrolledContainer[0].scrollHeight;
            scrolledContainer.animate({ scrollTop: height }, "slow");


        }

    }
    else {
        objtxtfield.val('');
        return;
    }


}
function isAllreadyExist(Keyword) {
    for (var i = 0; i < MultiKeywordContainer.length; i++) {
        if (MultiKeywordContainer[i] === Keyword) {
            return true;
        }

    }
    return false;

}
function deleteMultiCapsole(obj) {
    var text = jQuery(obj).prev().text();
    var index = MultiKeywordContainer.indexOf(text);
    var currentKeyword = servicetypeid == "3" ? jQuery("#txtKeywordStr") : jQuery("#txtKeyword");
    MultiKeywordContainer.splice(index, 1);
    jQuery(obj).parent().remove();

    MultiSerarchKeywordCounter = MultiSerarchKeywordCounter - 1;
    if (currentKeyword.parent().siblings().length <= 0) {
        currentKeyword.show();
        currentKeyword.css('width', 'auto');
        currentKeyword.val(defaultkeyword);

    }
    else {
        currentKeyword.show();
        //currentKeyword.focus();
        currentKeyword.val(defaultkeyword);

    }


}
function HideOrShowKeywords(UnitCategoryID) {

    if (jQuery('.auto-mulitple-capsole').length !== -1) {
        switch (UnitCategoryID) {
            case "5":
            case "3":
            case "4":
                jQuery('.auto-mulitple-capsole').hide();
                break;
            default:
                jQuery('.auto-mulitple-capsole').show();
                break;
        }

    }
    else {

    }
}
function checkMaxSize(minValue) {
    var filterCategoryId = "2";

    var selectedRanges = filterArray(allRangesData, function (rData) {
        return rData.CategoryId.toString() === filterCategoryId;
    });
    if (selectedRanges !== null && selectedRanges.length > 0 && selectedRanges[0].SliderValues !== '') {
        var sizeArray = selectedRanges[0].SliderValues.split(',');
        if (sizeArray.length > 0) {
            var maxSize = sizeArray[sizeArray.length - 1].replace('+', '');
            if (maxSize == minValue) {
                maxbedfromurl = sizeArray[sizeArray.length - 1];
            }
        }
    }
}

function ChangeControlState() {
    jQuery('.header').css("overflow", "visible");
    jQuery('.header').addClass("height-auto");
    if (isAdvanceControlsVisible) {

        //jQuery('#hmpgctrl').slideUp(600);
        //jQuery("#hmpgctrl").appendTo(jQuery(".control-header-place"));
        jQuery('.ui-autocomplete').addClass("ui-autocomplete2");
        jQuery("#hmpgctrl").addClass("hmpgctrl-fixed");
        jQuery(".newsearch-main-div-2").addClass('shrink_control');
        //jQuery('#hmpgctrl').slideDown(600);
    }
    else {

        jQuery('.ui-autocomplete').addClass("ui-autocomplete2");
        jQuery(".newsearch-main-div-2").css("margin-top", "0");
        jQuery('.sliding_controls').slideUp(400);
        //jQuery("#hmpgctrl").appendTo(jQuery(".control-header-place"));
        jQuery("#hmpgctrl").addClass("hmpgctrl-fixed");
        jQuery(".newsearch-main-div-2").addClass('shrink_control');
        //jQuery('#hmpgctrl').slideDown(600);


    }


    isControlSet = true;
}
function GetControlPrevState() {
    jQuery('.header').css("overflow", "hidden");
    jQuery('.header').removeClass("height-auto");
    jQuery('.sliding_controls').slideDown(400);
    jQuery('.ui-autocomplete').removeClass("ui-autocomplete2");
    jQuery("#hmpgctrl").removeClass("hmpgctrl-fixed");
    jQuery('.Main-search .pink-srch').removeClass("btnsalerent");
    jQuery(".newsearch-main-div-2").removeClass('shrink_control');
    // jQuery("#headerNewplace").prepend(jQuery("#hmpgctrl"));
    isControlSet = false;

}
function switchControls(cntrlID) {

}
function SelectCheckBoxes(objID, ValuesArr) {

    //var allChecked = jQuery("input[id*='unit_fixers'][type='checkbox']:checked");

    for (var i = 0; i < ValuesArr.length; i++) {

        jQuery("input[id*='" + objID + "'][type='checkbox']").each(function () {

            if (jQuery.trim(jQuery(this).val()) == jQuery.trim(ValuesArr[i])) {
                jQuery(this).prop('checked', true);
            }

        });
    }

}
function sortArr(values) {
    var length = values.length - 1;
    do {
        var swapped = false;
        for (var i = 0; i < length; ++i) {
            if (values[i] > values[i + 1]) {
                var temp = values[i];
                values[i] = values[i + 1];
                values[i + 1] = temp;
                swapped = true;
            }
        }
    }
    while (swapped == true)
}
function setUnitTypeForStr(data) {
    var unitType = "";
    switch (data.split('|')[4]) {
        case "1":
            unitType = "Apartment";
            break;
        case "2":
            unitType = "Villa";
            break;
        case "17":
            unitType = "Townhouse";
            break;
        case "1,2,17":
            unitType = "All";
            break;
        default:

    }
    jQuery("#ddlUnitTypeForResComCat").data("selectBox-selectBoxIt").selectOption(TrimString(unitType));
}




