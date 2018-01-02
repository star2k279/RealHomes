PageNo = 0;

jQuery(document).ready(function () {

    PageNo = 1;
    GetBlogPosts();
})

function GetBlogPosts()
{ //'@Url.Action("GetBlogPostList", "BlogCMS")'
    $.ajax({
        url: '/RHomes/umbraco/surface/BlogCMS/GetBlogPostList',
        async: false,
        type: "get",
        data: {
            iPageno: PageNo, sSortName: ""
        },
        dataType: "html",
        traditional: true,
        contentType: "application/json; charset=utf-8",
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
    GetBlogPosts();
}