<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PollResults.aspx.cs" Inherits="WebApplication1.PollResults" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </uc:ToolkitScriptManager>

    <div align="center" style="padding-top: 20px;">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" 
                                Text="Poll Results"></asp:Label>
                            <br />
                            <asp:GridView ID="GridView1" runat="server" CssClass="footable" AutoGenerateColumns="false"
                                Style="max-width: 500px" onrowdatabound="GridView1_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="UserName" HeaderText="User Name" />
                                    <asp:BoundField DataField="Vote" HeaderText="Vote" />
                                    <asp:BoundField DataField="Points" HeaderText="Points" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <table cellpadding="0" cellspacing="0" width="100%" class="footable">
                                        <tr style="background-color: #DCE9F9;">
                                            <th class="hidden-xs">
                                                <b>User Name</b>
                                            </th>
                                            <th>
                                                <b>Vote</b>
                                            </th>
                                            <th class="hidden-xs">
                                                <b>Points</b>
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="center" style="text-align: center;">
                                                <b>No Records Found</b>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
<%--                <div class="form-group">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New Poll Option" OnClick="Add" class="btn btn-success" />
                        </div>
                    </div>
                </div>--%>
            </div>
        </div>
    </div>
 <asp:Button ID="btnBack" runat="server" Text="Back" class="btn btn-success" 
        onclick="btnBack_Click"></asp:Button>

    <%--lnkFake Link Button for mpeAddUpdate ModalPopup as TargetControlID--%>
    <%--<asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>--%>
 
    <%--pnlAddUpdateDetails Panel With Design--%>
<%--    <asp:Panel ID="pnlAddUpdateDetails" runat="server" CssClass="modalPopup"
        Style="display: none;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="modal-header">
                <asp:Label ID="lblHeading" runat="server" CssClass="modal-title"></asp:Label>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-4">
                            <label for="txtOptionDesc">
                                Poll Option Description</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtOptionDesc" runat="server" CssClass="form-group" placeholder="Poll Option Description"
                                Width="150px"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:RequiredFieldValidator ID="rfvOptionDesc" Display="Dynamic" ValidationGroup="GenericValidationGroup"
                                ErrorMessage="Required" ControlToValidate="txtOptionDesc" runat="server" ForeColor="Red" />
                        </div>
                    </div>
                </div>

                <div class="form-group">
                    <div class="row">
                        <div class="col-md-4">
                            <label for="txtPollValue">
                                Poll Value</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtPollValue" runat="server" CssClass="form-group" placeholder="Poll Value"
                                Width="150px"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Dynamic" ValidationGroup="GenericValidationGroup"
                                ErrorMessage="Required" ControlToValidate="txtPollValue" runat="server" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-4">
                            <label for="txtIsCorrectAnswer">
                                Correct Answer</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtIsCorrectAnswer" runat="server" CssClass="form-group" placeholder=""
                                Width="150px"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                        </div>
                    </div>
                </div>
            </div>
            <div align="center" class="modal-footer">
                <div class="row">
                    <div class="col-md-12">
                        <asp:HiddenField ID="hfAddEditId" runat="server" Value="0" />
                        <asp:HiddenField ID="hfAddEdit" runat="server" Value="ADD" />
                        <asp:Button ID="btnSave" runat="server" Text="ADD" OnClick="Save" class="btn btn-success"
                            ValidationGroup="GenericValidationGroup"></asp:Button>
                        <button id="btnCancel" runat="server" class="btn btn-primary">
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>--%>
 
     <%--mpeAddUpdate Modal Popup Extender For pnlAddUpdateDetails--%>
<%--    <uc:ModalPopupExtender ID="mpeAddUpdate" runat="server" PopupControlID="pnlAddUpdateDetails"
        TargetControlID="lnkFake" BehaviorID="mpeAddUpdate" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </uc:ModalPopupExtender>--%>
 
    <%--lnkFake1 Link Button for mpeDelete ModalPopup as TargetControlID--%>
 <%--   <asp:LinkButton ID="lnkFake1" runat="server"></asp:LinkButton>--%>
 
    <%--pnlDelete Panel With Design--%>
    <%--<asp:Panel ID="pnlDelete" runat="server" CssClass="modalPopup" Style="display: none;">
        <div id="Div1" runat="server" class="header">
        </div>
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="form-group modal-body">
                <div class="row">
                    <div class="col-md-12">
                        Do you Want to delete this record ?
                    </div>
                </div>
            </div>
        </div>
        <div align="right" class="modal-footer">
            <div class="row">
                <div class="col-md-12">
                    <asp:HiddenField ID="hfDeleteId" runat="server" Value="0" />
                    <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="Yes" class="btn btn-danger">
                    </asp:Button>
                    <button id="btnNo" runat="server" class="btn btn-default">
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </asp:Panel>--%>
 
   <%-- mpeDelete Modal Popup Extender For pnlDelete--%>
<%--    <uc:ModalPopupExtender ID="mpeDelete" runat="server" PopupControlID="pnlDelete"
        TargetControlID="lnkFake1" BehaviorID="mpeDelete" CancelControlID="btnNo"
        BackgroundCssClass="modalBackground">
    </uc:ModalPopupExtender>--%>

</asp:Content>
