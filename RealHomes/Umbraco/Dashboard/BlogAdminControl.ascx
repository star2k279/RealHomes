<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BlogAdminControl.ascx.cs" Inherits="usercontrol_BlogAdminControl" %>
<%@ Import Namespace="umbraco.cms.businesslogic.web" %>
 
<table>
    <thead>
        <tr>
            <th></th>
        </tr>
    </thead>
    <tbody>
        <asp:Repeater ID="rptComments" runat="server"  OnItemCommand="rptComments_ItemCommand" >
            <HeaderTemplate>
                <tr>
                  <%--  <td style="width: 30px;"><strong>Select</strong></td>--%>
                    <td style="width: 150px;"><strong>Name</strong></td>
                    <td style="width: 400px;"><strong>Comment</strong></td>
                         <td style="width: 200px;"><strong>Post</strong></td>
                    <td colspan="2"><strong>Actions</strong></td>
                </tr>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <asp:HiddenField Value='<%# Eval("id")%>' ID="CommentId" runat="server" />
                   <%-- <td>
                        <asp:CheckBox ID="checkbox" class="listCheckbox" runat="server" /></td>--%>
                    <td><%#  ((Document)(Container.DataItem)).getProperty("smartBlogName").Value%></td>
                    <td><%#  ((Document)(Container.DataItem)).getProperty("smartBlogComment").Value%></td> 
                     <td><%# getPost(Convert.ToInt32(Eval("id"))).getProperty("smartBlogTitle").Value %></td>
                    <td>
                        <asp:LinkButton ID="Approve" runat="server" Text="Approve" OnClientClick="if(!confirm('Approve this comment?\n\nAre you sure?'))return false;" CommandName="ApproveComment" CommandArgument='<%# Eval("Id") %>' /></td>
                    <td>
                        <asp:LinkButton ID="Delete" runat="server" Text="Delete" OnClientClick="if(!confirm('Delete this comment?\n\nAre you sure?'))return false;" CommandName="DeleteComment" CommandArgument='<%# Eval("Id") %>' /></td>
                </tr>
            </ItemTemplate>

        </asp:Repeater>
    </tbody>
</table>
