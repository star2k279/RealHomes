<%@ Control Language="C#" AutoEventWireup="true" Inherits="SmartBlogLibraries.smartBlogManagement" Codebehind="smartBlogManagement.ascx.cs" %>
<%@ Register TagPrefix="umb" Namespace="ClientDependency.Core.Controls" Assembly="ClientDependency.Core" %>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<div class="propertypane">
    <div class="dashboardWrapper">
        <h2>Smart Blog Management</h2>
        <h3>Comments</h3>
        <span>Manage new comments posted in all blogs on the site.</span>
    
        <asp:UpdatePanel ID="commentListUpdater" runat="server">
            <ContentTemplate>
                <div style="padding: 10px 0 0 0;">
                    <input id="smartSelectAll" type="checkbox" />
                    <asp:Button ID="ApproveSelected" runat="server" Text="Approve Selected" OnClientClick="if(!confirm('Approve the selected comments?\n\nAre you sure?'))return false;" OnClick="ApproveSelected_Click" />
                    <asp:Button ID="DeleteSelected" runat="server" Text="Delete Selected" OnClientClick="if(!confirm('Delete the selected comments?\n\nAre you sure?'))return false;" OnClick="DeleteSelected_Click" />
                    <asp:Button ID="RefreshList" runat="server" Text="Refresh Comments" onclick="RefreshComments_Click" />
                </div>
                <hr />
                <table>
                    <asp:Repeater runat="server" id="rptComments" OnItemCommand="rptComments_ItemCommand" onitemdatabound="rptComments_ItemDataBound">
                        <HeaderTemplate>
                            <tr>
                                <td style="width: 30px;"><strong>Select</strong></td>
                                <td style="width: 150px;"><strong>Name</strong></td>
                                <td style="width: 400px;"><strong>Comment</strong></td>
                                <td style="width: 200px;"><strong>Post</strong></td>
                                <td colspan="2"><strong>Actions</strong></td>
                            </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <asp:HiddenField Value='<%# Eval("id")%>' ID="CommentId" runat="server" />
                                <td><asp:CheckBox ID="checkbox" class="listCheckbox" runat="server" /></td>
                                <td><%# getPropertyValue(Convert.ToInt32(Eval("id")), "smartBlogName")%></td>
                                <td><%# getPropertyValue(Convert.ToInt32(Eval("id")), "smartBlogComment")%></td>
                                <td><%# getPost(Convert.ToInt32(Eval("id"))).GetProperty("smartBlogTitle").Value %></td>
                                <td><asp:LinkButton ID="Approve" runat="server" Text="Approve" OnClientClick="if(!confirm('Approve this comment?\n\nAre you sure?'))return false;" CommandName="ApproveComment" CommandArgument='<%# Eval("Id") %>' /></td>
                                <td><asp:LinkButton ID="Delete" runat="server" Text="Delete" OnClientClick="if(!confirm('Delete this comment?\n\nAre you sure?'))return false;" CommandName="DeleteComment" CommandArgument='<%# Eval("Id") %>' /></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <td colspan="6">
                                <asp:Label ID="lblEmptyData" Text="There are no new comments to report." runat="server" Visible="false"></asp:Label>
                            </td>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <hr />
        <h3>Settings</h3>
        <span>Set up additional global settings for all your blogs on this website.</span><br />
        <table>
            <tr>
                <td>Default post author name:</td>
                <td><asp:TextBox ID="defaultAuthorName" runat="server" /></td>
            </tr>
            <tr>
                <td>Email commenter on approval:</td>
                <td><asp:CheckBox ID="approvalEmail" runat="server" /></td>
            </tr>
            <tr>
                <td>Email moderator on comment:</td>
                <td><asp:TextBox ID="commentEmailAddress" runat="server" /></td>
            </tr>
            <tr>
                <td>Automated email from address:</td>
                <td><asp:TextBox ID="robotEmailAddress" runat="server" /></td>
            </tr>
            <tr>
                <td>Auto approve comments:</td>
                <td><asp:CheckBox ID="autoApproveComments" runat="server" /></td>
            </tr>
            <tr>
                <td>Disable comments:</td>
                <td><asp:CheckBox ID="disableComments" runat="server" /></td>
            </tr>
            <tr>
                <td>Use summary on post list:</td>
                <td><asp:CheckBox ID="useSummary" runat="server" /></td>
            </tr>
            <tr>
                <td>Disable Facebook integration on comment form:</td>
                <td><asp:CheckBox ID="disableFacebookIntegration" runat="server" /></td>
            </tr>
        </table>
        <div style="padding: 10px 0 0 0;">
            <asp:Button ID="saveSettings" runat="server" Text="Save Settings" OnClick="SaveSettings_Click" />
        </div>

        <script type="text/javascript">
            $(document).ready(function () {
                $('#smartSelectAll').toggle(function () {
                    $('.listCheckbox input:first-child').attr('checked', 'checked');
                }, function () {
                    $('.listCheckbox input:first-child').removeAttr('checked');
                })
            })
        </script>
    </div>
</div>