var TextBoxFocusOut = "";
var dataToEncry = "";
var features = "";
var Collection = "";
jQuery(document).ready(function () {
    jQuery('#conrfirm_Str').click(function () {
        if (dateFrom.value == "" || dateFrom.value == "Check-in date") { alert("Please enter check-in date"); return false; }
        else if (dateFrom2.value == "" || dateFrom2.value == "Check-out date") { alert("Please enter check-out date"); return false; }
        else {
            jQuery('#str_dates').bPopup().close();
            validate(document.getElementById("btnStr"));
        }
    });
});
function validate(obj) {

    getFeatures();
    getFixtures();
    getUnitView();



    if (jQuery('#txtKeyword').val() != '' && TextBoxFocusOut != '') {


        if (Collection != "Enter location") {
            Collection += ',' + TextBoxFocusOut;
            jQuery('#txtKeyword').val(Collection);

        }

            // alert(Collection);
        else {
            jQuery('#txtKeyword').val(TextBoxFocusOut);
        }
    }
    else if (jQuery('#txtKeyword').val() == '' && TextBoxFocusOut != '') {
        jQuery('#txtKeyword').val(TextBoxFocusOut);

    }

    var dateFrom = document.getElementById("dateFrom"); var dateFrom2 = document.getElementById("dateFrom2");
    if (jQuery('#imageHTml').length > 0) {
        jQuery('#imageHTml').html('');
    }
    if (categoryid == "3" && !Headercontrol) {
        var UnitType = document.getElementById("ddlUnitType"); if (UnitType.value == "-1") { alert("Please select unitType"); return false; }
        else if (dateFrom.value == "" || dateFrom.value == "Check-in date") { alert("Please enter check-in date"); return false; }
        else if (dateFrom2.value == "" || dateFrom2.value == "Check-out date") {
            alert("Please enter check-out date"); return false;
        }
    }
    if (categoryid == "5") {
        var txtKeyword = document.getElementById("txtKeywordRefNo"); if (txtKeyword.value == "" || txtKeyword.value == "Unit Ref# e.g. AP5678,AP5679") { alert("Please enter ref no(s)."); return false; }
    }
    if (categoryid == "1" || categoryid == "2" || categoryid == "10") {

        if (countryid != 216) {

            //if (parseFloat(document.getElementById('ddlminPrice').value) > 10 && parseFloat(document.getElementById('ddlmaxPrice').value) > 10) {
            //    if (parseFloat(document.getElementById('ddlminPrice').value) > parseFloat(document.getElementById('ddlmaxPrice').value)) {
            //        alert("Max price should not be less than min price."); return false;
            //    }
            //}
            //if (parseFloat(document.getElementById('ddlminPrice').value) <= 10 && parseFloat(document.getElementById('ddlmaxPrice').value) <= 10) {

            //    if (parseFloat(document.getElementById('ddlminPrice').value) > parseFloat(document.getElementById('ddlmaxPrice').value)) {
            //        alert("Max price should not be less than min price."); return false;
            //    }
            //}

        }
            //ampenment in code for oman
        else {


            var ddlminprice = (document.getElementById('ddlminPrice').value);
            var ddlmaxprice = (document.getElementById('ddlmaxPrice').value);


            if (ddlminprice.indexOf('k') != -1) {
                ddlminprice = ddlminprice.split(' ')[0] * 1000;
            }

            else if (ddlminprice.indexOf('m') != -1) {
                ddlminprice = ddlminprice.split(' ')[0] * 1000000;
            }
            else {
                ddlminprice = ddlminprice;
            }



            if (ddlmaxprice.indexOf('k') != -1) {
                ddlmaxprice = ddlmaxprice.split(' ')[0] * 1000;
            }

            else if (ddlmaxprice.indexOf('m') != -1) {
                ddlmaxprice = ddlmaxprice.split(' ')[0] * 1000000;
            }
            else {
                ddlmaxprice = ddlmaxprice;
            }



            if (parseFloat(ddlminprice) > 10 && parseFloat(ddlmaxprice) > 10) {
                if (parseFloat(ddlminprice) > parseFloat(ddlmaxprice)) {
                    alert("Max price should not be less than min price."); return false;
                }
            }
            if (parseFloat(ddlminprice) <= 10 && parseFloat(ddlmaxprice) <= 10) {

                if (parseFloat(ddlminprice) > parseFloat(ddlmaxprice)) {
                    alert("Max price should not be less than min price."); return false;
                }
            }

        }

    }
    if (jQuery('#tags').length > 0) {
        if (jQuery('#tags').val() == "Enter school name..." || jQuery('#tags').val() == "Enter metrostation name..." || jQuery('#tags').val() == "") {
            alert("Please enter name.");
            return false;
        }
    }

    var serviceControl = document.getElementById("ddlServicetype");
    var ddlobj = document.getElementById("ddlUnitTypeForResComCat");
    switch (jQuery(obj).attr("Value")) {

        case "1":
            categoryid = ddlobj.options[ddlobj.selectedIndex].getAttribute('catid');
            servicetypeid = "1";
            break;

        case "2":
            categoryid = ddlobj.options[ddlobj.selectedIndex].getAttribute('catid');
            servicetypeid = "2";
            break;

        case "3":
            categoryid = "3";
            servicetypeid = "3";
            break;
        case "4":
            categoryid = "4";
            break;
        case "5":
            categoryid = "10";
            servicetypeid = "5";
            break;
        default:
            break;

    }
    criteriaUpdate();
    if ($(obj).attr("id") === "btnStr") {
        RedirectholidayHomes();
    }
    if (typeof $("#ddlServicetype") !== "undefined" && $("#ddlServicetype").val() === "3") {
        RedirectholidayHomes();
    }
    else {
        GetEncryptedData();
        return true;
    }


}
function RedirectholidayHomes() {
    var locationNames = jQuery("#txtKeyword").val();
    var servicePath = '/local/Services/UnitService.svc/getStrLocationCode';
    var locationIds = "";
    params = JSON.stringify({
        LocationName: locationNames === "" ? "lkjljl" : locationNames
    });
    AjaxCall(servicePath, params, OnSuccessgetStrLocationCode, null, null, null, null);
    function OnSuccessgetStrLocationCode(data) {
        var strKeyWord = "";
        if (data !== "") {
            strKeyWord = data;
        }
        var ddlMinPrice = jQuery("#ddlminPrice").val();
        var ddlMaxPrice = jQuery("#ddlmaxPrice").val();
        var ddlMinBed = jQuery('#ddlminbedsize').val() === "All" ? "-1" : jQuery('#ddlminbedsize').val();
        var ddlMaxBed = jQuery('#ddlBedroomsOrSize') === "All" ? "All" : jQuery('#ddlBedroomsOrSize').val();
        var dateFrom = jQuery("#hiddateFrom").val().indexOf('Check') > -1 ? "" : jQuery("#hiddateFrom").val();
        var dateTo = jQuery("#hiddateFrom2").val().indexOf('Check') > -1 ? "" : jQuery("#hiddateFrom2").val();
        var url = "http://holidayhomes.bhomes.com/property/search.aspx?l=" + strKeyWord + '&minp=' + ddlMinPrice + '&maxp=' + ddlMaxPrice + '&minb=' + ddlMinBed + '&maxb=' + ddlMaxBed + '&chk-in=' + dateFrom + '&chk-out=' + dateTo;
        setTimeout(function () { document.location.href = url }, 500);
        //window.Location = url;
        return false;
    }

}

function GetEncryptedData() {

    var unitType;
    if (jQuery("#hidUnitTypeForResComCat").val() != undefined) {
        unitType = jQuery("#hidUnitTypeForResComCat").val();
    }
    else {
        unitType = jQuery('#hidddlUnitTypeForCategories').val();
    }


    var servicePath = '/local/Services/UnitService.svc/RedirectionWithEncryption';
    dataToEncry = JSON.stringify({
        hdncurrency: jQuery("#hdncurrency").val(),
        hidtxtKeyword: jQuery("#txtKeyword").val(),
        hidCategory: jQuery("#hidCategory").val(),
        txtKeywordRefNoAdvanceOption: (jQuery("#txtKeywordRefNoAdvanceOption").is(':visible') && jQuery("#txtKeywordRefNoAdvanceOption").val() != undefined) ? jQuery("#txtKeywordRefNoAdvanceOption").val() : jQuery("#txtKeywordRefNo").val(),
        hidBedroomsOrSize: jQuery("#hidBedroomsOrSize").val(),
        hidminbedsize: jQuery("#hidminbedsize").val(),
        hidUnitTypeForResComCat: unitType,
        hidServiceID: jQuery("#hidServiceID").val(),
        hidminPrice: jQuery("#hidminPrice").val(),
        hidmaxPrice: jQuery("#hidmaxPrice").val(),
        hiddateFrom: jQuery("#hiddateFrom").val(),
        hiddateFrom2: jQuery("#hiddateFrom2").val(),
        hidCityStr: jQuery("#hidCityStr").val(),
        hidUnitType: jQuery('#hidUnitType').val(),
        hidBedroomsStr: jQuery('#hidBedroomsStr').val(),
        hidDistrictStr: jQuery("#hidDistrictStr").val(),
        hidCommunityStr: jQuery("#hidCommunityStr").val(),
        hidAgentID: jQuery("#hidAgentID").val(),
        hidTypeOfService: jQuery("#hidTypeOfService").val(),
        hidAreaOfSpeciality: jQuery("#hidAreaOfSpeciality").val(),
        hidAgentCity: jQuery("#hidAgentCity").val(),
        Feature: jQuery('#features').val(),
        isheaderControl: Headercontrol,
        Unitdevelopment: jQuery("#ddldeveopment").val(),
        fitingFixtures: jQuery("#unitFixture").val(),
        UnitViews: jQuery("#Unitview").val(),
        currentUrl: (jQuery("#hidcurrentURL").val() == undefined || jQuery("#hidcurrentURL").val() == "") ? "/property/newsearch.xhtml" : jQuery("#hidcurrentURL").val(),
        drawpaths: jQuery("#DrawPaths").val(),
        drawRadius: jQuery("#drawRadius").val(),
        drawMapAttrtibutes: jQuery("#drawMapAttrtibutes").val(),
        SearchViewMode: GetViewMode(),
        selectedLocType: selectedlocationtype

    });
   
    AjaxCall(servicePath, dataToEncry, OnSuccessRedirectionWithEncryption, null, null, null, null);
}
function GetViewMode() {
    if (document.URL.indexOf("mapsearch") > 0 && document.URL.indexOf("listview") === -1) {
        return "false";
    }
    else {
        getParameterByName("listview", 'true')
    }

}
function OnSuccessRedirectionWithEncryption(result) {
    window.location.href = result;
    //PostForm(result, 'Post');

}
function setServiceType(obj) {
    criteriaUpdate();
    jQuery("#hidServiceID").val(jQuery(obj).attr("Value"))

}
function getFeatures() {

    var checkArr = [];
    var checkCounts = 0;
    var allChecked = $("input[id*='cblFeaturesList'][type='checkbox']:checked");
    jQuery("input[id*='cblFeaturesList'][type='checkbox']:checked").each(function () {
        checkCounts = checkCounts + 1;

        if (checkCounts > 1 && checkCounts < checkCounts.lengh) {
            checkArr.push(jQuery(this).val() + ",");
        }
        else {
            checkArr.push(jQuery(this).val());
        }
    });
    if (checkArr.length > 0) {
        jQuery('#features').val(checkArr);
    }
    else {

    }


}
function getUnitView() {

    var UnitviewArr = [];
    var checkCounts = 0;
    var allChecked = $("input[id*='unit_views'][type='checkbox']:checked");
    jQuery("input[id*='unit_views'][type='checkbox']:checked").each(function () {
        checkCounts = checkCounts + 1;

        if (checkCounts > 1 && checkCounts < checkCounts.lengh) {
            UnitviewArr.push(jQuery(this).val() + ",");
        }
        else {
            UnitviewArr.push(jQuery(this).val());
        }
    });
    if (UnitviewArr.length > 0) {
        jQuery('#Unitview').val(UnitviewArr);

    }
    else {

    }

}
function getFixtures() {

    var FixtureArr = [];
    var checkCounts = 0;
    var allChecked = $("input[id*='unit_fixers'][type='checkbox']:checked");
    jQuery("input[id*='unit_fixers'][type='checkbox']:checked").each(function () {
        checkCounts = checkCounts + 1;

        if (checkCounts > 1 && checkCounts < checkCounts.lengh) {
            FixtureArr.push(jQuery(this).val() + ",");
        }
        else {
            FixtureArr.push(jQuery(this).val());
        }
    });
    if (FixtureArr.length > 0) {
        jQuery('#unitFixture').val(FixtureArr);
    }
    else {

    }

}


function autoHide(ctrl) {
    if (ctrl.value == "Enter location") {
        try {
            ctrl.value = ''; ctrl.style['color'] = '#3F6095';
        }
        catch (err) {
        }
    }
    else if (ctrl.value == '') {
        try {
            if (jQuery('.keyword-capsule').length <= 0) { ctrl.style['color'] = '#3F6095'; ctrl.value = 'Enter location'; } else { ctrl.value = ''; }
        }
        catch (err) {

        }
    }


    else if (ctrl.value != '' && ctrl.value != "Enter location") {


        TextBoxFocusOut = ctrl.value;

    }
}
function autoHideFontColorUnChanged(ctrl) {
    if (ctrl.value == "Enter location") {
        try {
            ctrl.value = '';
        }
        catch (err) {
        }
    }
    else if (ctrl.value == '') {
        try {
            if (jQuery('.keyword-capsule').length <= 0) { ctrl.value = 'Enter location'; } else { ctrl.value = ''; }
        }
        catch (err) {
        }
    }
}
function autoHideRefNo(ctrl) {
    if (ctrl.value == "Unit Ref# e.g. AP5678,AP5679") {
        try {
            ctrl.value = ''; ctrl.style['color'] = '#3F6095';
        }
        catch (err) {
        }
    }
    else if (ctrl.value == '') {
        try {
            if (jQuery('.keyword-capsule').length <= 0) { ctrl.style['color'] = '#3F6095'; ctrl.value = 'Unit Ref# e.g. AP5678,AP5679'; } else { ctrl.value = ''; }
        }
        catch (err) {
        }
    }
}
function autoHideAgent(ctrl) {
    if (ctrl.value == "Search By Agent e.g. Paul Smith") {
        try {
            ctrl.value = ''; ctrl.style['color'] = '#3F6095';
        }
        catch (err) {
        }
    }
    else if (ctrl.value == '') {
        try {
            if (jQuery('.keyword-capsule').length <= 0) { ctrl.style['color'] = '#3F6095'; ctrl.value = 'Search By Agent e.g. Paul Smith'; } else { ctrl.value = ''; }
        }
        catch (err) {
        }
    }
}


