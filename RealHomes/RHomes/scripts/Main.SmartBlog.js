// 'getScript' loads scripts after the page has loaded so the page can render quicker 
function getScript(url, success) {
    var script = document.createElement('script');
    script.type = "text/javascript";
    script.async = true;
    script.src = url;
    var head = document.getElementsByTagName('head')[0],
        done = false;
    script.onload = script.onreadystatechange = function () {
        if (!done && (!this.readyState || this.readyState == 'loaded' || this.readyState == 'complete')) {
            done = true;
            if (typeof success != 'undefined') { success(); }
            script.onload = script.onreadystatechange = null;
            head.removeChild(script);
        }
    };
    head.appendChild(script);
}

// --------------- Query actions --------------- //
getScript('https://ajax.googleapis.com/ajax/libs/jquery/2.0.0/jquery.min.js', function () {
    // -------- MVC Client Side Validation ---------- //
    getScript('http://ajax.aspnetcdn.com/ajax/jquery.validate/1.11.1/jquery.validate.min.js');
    getScript('/scripts/jquery.validate.unobtrusive.min.js');

    // Archive
    if ($(".smartArchive").length > 0) {
        $(".smartArchive a").on("click", function (e) {
            var link = $(this);
            var listItem = $(link.parent());
            var subList = listItem.children("ul");

            if (subList.length > 0) {
                e.preventDefault();

                if (listItem.hasClass("open")) {
                    listItem.removeClass("open");
                    subList.hide();
                }
                else {
                    listItem.addClass("open");
                    subList.show();
                }
            }
        });
    }

    // Loads in the facebook api
    getScript('https://connect.facebook.net/en_US/all.js', function () {
        FB.init({
            appId: '374162762699021',   // App ID from the app dashboard, make sure the app is live and your website domain has been set within it.
            status: true,               // Check Facebook Login status
            xfbml: true,                // Look for social plugins on the page
            oauth: true
        });

        // When the facebook button is clicked (if it exists), then login the user
        if ($('#facebookButton').length > 0) {
            $('#facebookButton').click(function (e) {
                e.preventDefault();
                login();
            });
        }

        // The login function to retrieve and set the user details into the form.
        function login() {
            FB.getLoginStatus(function (response) {
                console.log('Welcome!  Fetching your information.... ');
                FB.api('/me', function (response) {
                    console.log('Good to see you, ' + response.name + '.');
                });

                if (response.status === 'connected') {
                    // User is already authorised and logged in
                    FB.api('me?fields=name,email,website', function (user) {
                        if (user != null) {
                            console.log("You've already provided authorisation to this website");
                            $("#commentName").val(user.name);
                            $("#commentEmail").val(user.email);
                            $("#commentWebsite").val(user.website);
                        }
                    });
                } else if (response.status === 'not_authorized') {
                    // The user is logged in but has not authenticated the app
                    console.log("Looks like you've not provided access to this application")
                } else {
                    // The user has not logged into facebook so ask them to
                    FB.login(function (response) {
                        if (response.authResponse) {
                            if (response.authResponse.accessToken) {
                                FB.api('me?fields=name,email,website', function (user) {
                                    if (user != null) {
                                        $("#commentName").val(user.name);
                                        $("#commentEmail").val(user.email)
                                        $("#commentWebsite").val(user.website);
                                    }
                                })
                            }
                        }
                        else {
                            alert("You need to log in and authorise this website before you can autofill the form.");
                        }
                    }, { scope: 'email,user_website' });
                }
            }, true);
        }
    });

    // Listen for search request
    $('#smartBlogSearchForm').on('submit', function (e) {
        e.preventDefault();

        doSmartBlogSearch();
    });

    // Perform search
    function doSmartBlogSearch() {
        var address = document.URL.split('?')[0];
        var query = $("#smartSearchInput").val();

        // If no search term has been requested then alert the user,
        // else perform the search.
        if (query != "") {
            // If on a post, we need to redirect the user up a level too.
            if ($('#postPageBody').length > 0) {
                window.location = address.substr(0, address.lastIndexOf("/")) + "?q=" + query;
            }
            else {
                window.location = address + "?q=" + query;
            }
        }
        else {
            alert("Please provide a search term.")
        }
    }
});

// If any ajax call fails
function OnErrorCall(response) {
    alert(response.status + " " + response.statusText);
    console.log(response)
}