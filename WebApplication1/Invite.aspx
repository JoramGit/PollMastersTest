<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Invite.aspx.cs" Inherits="WebApplication1.Invite" EnableEventValidation="false" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </uc:ToolkitScriptManager>

 
    <%--lnkFake Link Button for mpeAddUpdate ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
 
    <%--pnlAddUpdateDetails Panel With Design--%>
    <asp:Panel ID="pnlAddUpdateDetails" runat="server" CssClass="modalPopup"
        Style="display:block;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="modal-header">
                <asp:Label ID="lblHeading" runat="server" CssClass="modal-title"></asp:Label>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-4">
                            <label for="txtEmail">
                                Invite</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-group" placeholder="Email invitation"
                                Width="150px"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:RequiredFieldValidator ID="rfvEmail" Display="Dynamic" ValidationGroup="GenericValidationGroup"
                                ErrorMessage="Required" ControlToValidate="txtEmail" runat="server" ForeColor="Red" />
                            <asp:RegularExpressionValidator ID="regexEmailValid" runat="server" Display="Dynamic" ValidationGroup="GenericValidationGroup" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ControlToValidate="txtEmail" ForeColor="Red" ErrorMessage="Invalid Email Format"></asp:RegularExpressionValidator>
                        </div>
                    </div>
                </div>
            </div>
            <div align="center" class="modal-footer">
                <div class="row">
                    <div class="col-md-12">
                        <asp:HiddenField ID="hfAddEditId" runat="server" Value="0" />
                        <asp:HiddenField ID="hfAddEdit" runat="server" Value="ADD" />
                        <asp:Button ID="btnSave" runat="server" Text="Send invitation" OnClick="Save" class="btn btn-success"
                            ValidationGroup="GenericValidationGroup"></asp:Button>
                        <button id="btnCancel" runat="server" class="btn btn-primary">
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
 
     <%--mpeAddUpdate Modal Popup Extender For pnlAddUpdateDetails--%>
    <uc:ModalPopupExtender ID="mpeAddUpdate" runat="server" PopupControlID="pnlAddUpdateDetails"
        TargetControlID="lnkFake" BehaviorID="mpeAddUpdate" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </uc:ModalPopupExtender>

</asp:Content>
