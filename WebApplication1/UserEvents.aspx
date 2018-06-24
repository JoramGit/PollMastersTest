<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserEvents.aspx.cs" Inherits="WebApplication1.UserEvents" EnableEventValidation="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <uc:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </uc:ToolkitScriptManager>
<br />
<div align="center" style="padding-top: 20px;">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Size="X-Large" 
                                Text="User Events"></asp:Label>
                            <br />
                            <asp:GridView ID="GridView1" runat="server" CssClass="footable" AutoGenerateColumns="false"
                                Style="max-width: 500px" onrowdatabound="GridView1_RowDataBound" 
                                onselectedindexchanged="GridView1_SelectedIndexChanged" 
                                DataKeyNames="EventId" onrowcommand="GridView1_RowCommand1"> 
                                <Columns>
                                    <asp:BoundField DataField="EventId" HeaderText="Id" />
                                    <asp:BoundField DataField="EventDesc" HeaderText="Event Name" />
                                    <asp:BoundField DataField="EventTypeName" HeaderText="Type" />
                                    <asp:TemplateField HeaderText="" Visible = "False">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUserId" runat="server" Text='<%# Eval("UserId") %>'></asp:Label>
                                            <%--<asp:Button ID="UserId" runat="server" Text="Users" CommandName="Users" CommandArgument='<%# Eval("EventId") %>'
                                                class="btn btn-primary" />--%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnUsers" runat="server" Text="Join" CommandName="Users" CommandArgument='<%# Eval("EventId") %>'
                                                class="btn btn-primary" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table cellpadding="0" cellspacing="0" width="100%" class="footable">
                                        <tr style="background-color: #DCE9F9;">
                                            <th class="hidden-xs">
                                                <b>ID</b>
                                            </th>
                                            <th>
                                                <b>Event Description</b>
                                            </th>
                                            <th class="hidden-xs">
                                                <b>Type</b>
                                            </th>
                                            <th>
                                                &nbsp;
                                            </th>
                                            <th>
                                                &nbsp;
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="5" align="center" style="text-align: center;">
                                                <b>No Records Found</b>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <%--<div class="form-group">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New Event" OnClick="Add" class="btn btn-success" />
                        </div>
                    </div>
                </div>--%>
            </div>
        </div>
    </div>
 
    <%--lnkFake Link Button for mpeAddUpdate ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
 
    <%--pnlAddUpdateDetails Panel With Design--%>
    <asp:Panel ID="pnlAddUpdateDetails" runat="server" CssClass="modalPopup"
        Style="display: none;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="modal-header">
                <asp:Label ID="lblHeading" runat="server" CssClass="modal-title"></asp:Label>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-4">
                            <label for="txtEventDesc">
                                Event Name</label>
                        </div>
                        <div class="col-md-5">
                            <asp:TextBox ID="txtEventDesc" runat="server" CssClass="form-group" placeholder="Event Name"
                                Width="150px"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:RequiredFieldValidator ID="rfvEventDesc" Display="Dynamic" ValidationGroup="GenericValidationGroup"
                                ErrorMessage="Required" ControlToValidate="txtEventDesc" runat="server" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-4">
                            <label for="ddlEventType">
                                Type</label>
                        </div>
                        <div class="col-md-5">
                            <asp:DropDownList ID="ddlEventType" runat="server" CssClass="form-group" placeholder="Type"
                                Width="150px"></asp:DropDownList>
<%--                            <asp:TextBox ID="txtEventType" runat="server" CssClass="form-control" placeholder="EventType"
                                Width="150px" ></asp:TextBox>--%>
                            <%--<uc:FilteredTextBoxExtender ID="ftEventType" runat="server" TargetControlID="txtEventType"
                                FilterType="Custom" ValidChars="1234567890.">
                            </uc:FilteredTextBoxExtender>--%>
                        </div>
                        <div class="col-md-3" >   
                            <%--<asp:RequiredFieldValidator ID="rfvEventType" Display="Dynamic" ValidationGroup="GenericValidationGroup"
                                ErrorMessage="Required" ControlToValidate="txtEventType" runat="server" ForeColor="Red" />--%>
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
    </asp:Panel>
 
     <%--mpeAddUpdate Modal Popup Extender For pnlAddUpdateDetails--%>
    <uc:ModalPopupExtender ID="mpeAddUpdate" runat="server" PopupControlID="pnlAddUpdateDetails"
        TargetControlID="lnkFake" BehaviorID="mpeAddUpdate" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </uc:ModalPopupExtender>
 
    <%--lnkFake1 Link Button for mpeDelete ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkFake1" runat="server"></asp:LinkButton>
 
    <%--pnlDelete Panel With Design--%>
    <asp:Panel ID="pnlDelete" runat="server" CssClass="modalPopup" Style="display: none;">
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
    </asp:Panel>
 
   <%-- mpeDelete Modal Popup Extender For pnlDelete--%>
    <uc:ModalPopupExtender ID="mpeDelete" runat="server" PopupControlID="pnlDelete"
        TargetControlID="lnkFake1" BehaviorID="mpeDelete" CancelControlID="btnNo"
        BackgroundCssClass="modalBackground">
    </uc:ModalPopupExtender>

</asp:Content>
