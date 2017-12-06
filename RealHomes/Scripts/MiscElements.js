var userResult = false;
var emailResult = false;


function checkPassword() {

    var pwd1 = $("#txtPassword").val();
    var pwd2 = $("#txtConfirmPassword").val();

    if (pwd1 != pwd2) {
        //alert('Password do not match.');
        //$("#txtPassword").focus();
        $("#lblPasswordError").empty().append('Passwords do not match.');
        $("#lblPasswordError").show();
    }
    else {
        $("#lblPasswordError").empty();
        $("#lblPasswordError").hide();
    }

}
/*function isPwdValid() {
    //var pwdreg = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{10}$/;
    var pwdreg = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$/;//here '^' and '\' are               shown invalid characters 
    var pwdval = $("#txtPassword").val();
    var span = $("#spanerror")[0];
    if (!pwdreg.test(pwdval)) {
        if (span == undefined) {
            $("#txtPassword").after('<span id="spanerror" class="error">Password must be 10 chars and include at least one upper case letter, one lower case letter, and one numeric digit.</span>');
        }
        else {
            $("#spanerror").text("Password must be 10 chars and include at least one upper case letter, one lower case letter, and one numeric digit.");
        }
        $("#txtPassword").focus();
    }
    else
    {
        $("#spanerror").empty();
    }
}

*/

function isEmailValid() {
   
    return $.ajax({
        url: '/RHomes/umbraco/surface/MembershipCMS/GetUserByEmail',
        async: false,
        type: "GET",
        data: {
            sEmail: $("#txtEmail").val()
        },
        dataType: "JSON",
        contentType: "application/json",
        error: function (data) {
            alert(data + ': Error getting data from ajax call');
        },
        success: function (data, textStatus, jqXHR) {
           emailResult = data
            
        }
    });

}

function isUserValid() {
   
    return $.ajax({
        url: '/RHomes/umbraco/surface/MembershipCMS/GetUserByName',
        async: false,
        type: "GET",
        data: {
            sName: $("#txtUserName").val()
        },
        dataType: "JSON",
        contentType: "application/json",
        error: function (data) {
            alert('Error getting data from ajax call');
        },
        success: function (data, textStatus, jqXHR) { userresult = data; }
    });
}


function validate()
{
    var error = false;
    var result = $.when(isUserValid(), isEmailValid()).done(function () {  
    
    if (userresult == true) {
        $("#lblUserError").empty().append('User name already exists. Please try another.');
        $("#lblUserError").show();
        error = true;
    }
    else {
        $("#lblUserError").empty().hide();
        
    }
    
    if (emailResult == true) {
        $("#lblEmailError").empty().append('User already exists with the selected email adress.');
        $("#lblEmailError").show();
        error = true;

    }
    else {
        $("#lblEmailError").empty().hide();
    }

    if (error == false)
        $("#registerForm").submit();
    });
  
    return false;
}