$(document).ready(function () {
    var url = "";
    //$("#txtEmail").pattern("[a-z0-9._%+-]+@@[a-z0-9.-]+\.[a-z]{2,3}$");
    
    $("#dialogEnquiry").dialog({
        title:"Site visit enquiry",
        autoOpen: false,
        resizable: false,
        height: 370,
        width: 550,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
        },
        buttons: {
            "Submit": function () {
                //var email = ("#txtEmail").value;

                if (ValidateEmail($("#txtEmail").val()) == false)
                {
                    $("#lblError").text("Enter correct email address.");
                    $("#lblError").show();
                }
                else {
                    $("#lblError").text("");
                    $("#lblError").hide();

                    //ajax call to controller function to save the enquiry
                    $.ajax({
                        url: '/RHomes/umbraco/surface/VisitEnquiryCMS/CreateEnquiry',
                        async: true,
                        type: "GET",
                        data: {
                            sRefNo: $("#txtRefNo").val(), sName: $("#txtName").val(), sEmail: $("#txtEmail").val(), sPropDetail: $("#txtPropDetail").val(), sEnqDetail: $("#txtEnqDetail").val()
                        },
                        dataType: "html",
                        traditional: true,
                        contentType: "application/json",
                        error: function (data) {
                            alert('Error getting data from ajax call');
                        },
                        success: function (data, textStatus, jqXHR) {
                            //alert(data + ': Enquiry saved successfully');
                            //console.log(data);
                            $("#lblError").empty().append(data + ': Enquiry saved successfully');
                            $("#lblError").show();
                            $(this).dialog("close");
                        }
                    });
                    //$(this).dialog("close");
                }

            },
            "Close": function () {
                $("#lblError").val("");
                $("#lblError").hide();
                $(this).dialog("close");
            }
        }

    });

    // Make an Offer dialog button 
    $("#dialogOffer").dialog({
        title: "Make an Offer",
        autoOpen: false,
        resizable: false,
        height: 370,
        width: 550,
        show: { effect: 'drop', direction: "up" },
        modal: true,
        draggable: true,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close").hide();
        },
        buttons: {
            "Submit": function () {
                //var email = ("#txtEmail").value;
                var financeValue;
                if ($("#financeYes").is(":checked"))
                    financeValue = true;
                else
                    financeValue = false;

                if (ValidateEmail($("#txtPersonEmail").val()) == false) {
                    $("#lblOfferError").text("Enter correct email address.");
                    $("#lblOfferError").show();
                }
                else {
                    $("#lblOfferError").text("");
                    $("#lblOfferError").hide();

                    //ajax call to controller function to save the enquiry
                    $.ajax({
                        url: '/RHomes/umbraco/surface/MakeAnOfferCMS/SaveOffer',
                        async: true,
                        type: "GET",
                        data: {
                            sRefNo: $("#txtRefNo").val(), sName: $("#txtPersonName").val(), sEmail: $("#txtPersonEmail").val(), OfferPrice: $("#txtOfferPrice").val(), financing: financeValue
                        },
                        dataType: "html",
                        traditional: true,
                        contentType: "application/json",
                        error: function (data) {
                            $("#lblOfferError").empty().append(data + ': Cannot save Offer at this moment.');
                            $("#lblOfferError").show();
                        },
                        success: function (data, textStatus, jqXHR) {
                            //alert(data + ': Enquiry saved successfully');
                            //console.log(data);
                            $("#lblError").empty().append(data + ': Offer saved successfully');
                            $("#lblError").show();
                            $(this).dialog("close");
                        }
                    });
                    //$(this).dialog("close");
                }

            },
            "Close": function () {
                $("#lblOfferError").val("");
                $("#lblOfferError").hide();
                $(this).dialog("close");
            }
        }

    });

    /*
    $(document).ready(function () {                            
    $("#radio_1, #radio_2", "#radio_3").change(function () {
        if ($("#radio_1").is(":checked")) {
            $('#div1').show();
        }
        else if ($("#radio_2").is(":checked")) {
            $('#div2').show();
        }
        else 
            $('#div3').show();
    });        
});
    */


})

function ValidateEmail(mail) {
    
    //var pattern = "[a-z0-9._%+-]+@@[a-z0-9.-]+\.[a-z]{2,3}$";
    if (/[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,3}$/.test(mail)) {
        return (true)
    }
    else {
        return (false)
    }
}

function OpenDialog(refNo, title) {
    $("#txtRefNo").val(refNo);
    $("#lblPropDetail").text(title + '-' + refNo);
    $("#txtName").val();
    $("#txtEmail").val("");
    $("#txtPropDetail").val(title + '-' + refNo);
    $("#txtEnqDetail").val("");

    $("#lblError").val("");
    $("#lblError").hide();

    $("#dialogEnquiry").dialog("open");

}

function OpenOfferDialog(refNo, title) {
    $("#txtRefNo").val(refNo);
    $("#lblOfferProp").text(title + '-' + refNo);
    $("#txtPersonName").val();
    $("#txtPersonEmail").val("");
    $("#txtOfferPrice").val("");
    $("#financeYes").prop("checked", true)

    $("#lblOfferError").val("");
    $("#lblOfferError").hide();

    $("#dialogOffer").dialog("open");

}