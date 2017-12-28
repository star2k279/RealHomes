using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Umbraco.Web;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using System.Xml;

using umbraco.presentation.nodeFactory;
using umbraco.cms.businesslogic.web;



public partial class usercontrol_BlogAdminControl : System.Web.UI.UserControl
{
    private static List<Document> listNode = new List<Document>();
    bool sendApproveEmail = false;

    public  List<Document> GetDescendantOrSelfNodeList(List<Document> listNode , Document node, string nodeTypeAlias)
    { 
        if (node.ContentType.Alias == nodeTypeAlias && !node.Published ) listNode.Add(node);

        foreach (Document childNode in node.Children )
        {
            listNode =  GetDescendantOrSelfNodeList(listNode , childNode, nodeTypeAlias);
        }

        return listNode;
    }



    public void rptComments_ItemCommand(object Sender, RepeaterCommandEventArgs e)
    {
        if (e.CommandName.ToString() == "DeleteComment" && e.CommandArgument.ToString() != "")
        {
            deleteComment(Convert.ToInt32(e.CommandArgument.ToString()));
        }
        else if (e.CommandName.ToString() == "ApproveComment" && e.CommandArgument.ToString() != "")
        {
            approveComment(Convert.ToInt32(e.CommandArgument.ToString()));
        }

        
    }


    private void approveComment(int commentId)
    {
        Document comment = new Document(commentId);
        comment.Publish(SmartBlogLibraries.Helpers.Cms.author);
        umbraco.library.UpdateDocumentCache(comment.Id);

        refreshDT();

    }

    public Document getPost(int nodeId)
    {
        Document comment = new Document(nodeId);

        return new Document(comment.ParentId);
    }


    private void deleteComment(int commentId)
    {
        Document comment = new Document(commentId);
        if (comment.Published)
        {
            comment.UnPublish();
            umbraco.library.UnPublishSingleNode(comment.Id);
        }

        comment.delete();
        refreshDT();
        //umbraco.library.UpdateDocumentCache(Convert.ToInt32(e.CommandArgument.ToString()));
    }

    private void refreshDT()
    {
        Document aaa = new Document(1378);
        // we are passing root node so that it can search through nodes with alias as DiaryEventItems
        List<Document> diaryEventItems = GetDescendantOrSelfNodeList(new List<Document>(), aaa, "SmartBlogComment");



        rptComments.DataSource = diaryEventItems;


        rptComments.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
        Document aaa = new Document(1378);
        // we are passing root node so that it can search through nodes with alias as DiaryEventItems
        List<Document> diaryEventItems = GetDescendantOrSelfNodeList(new List<Document>(),aaa, "SmartBlogComment");



        rptComments.DataSource = diaryEventItems;


        rptComments.DataBind();
    }
}