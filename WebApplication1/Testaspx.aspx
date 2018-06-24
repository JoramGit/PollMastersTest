<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Testaspx.aspx.cs" Inherits="WebApplication1.Testaspx" %>
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
                            <asp:GridView ID="GridView1" runat="server" CssClass="footable" AutoGenerateColumns="false"
                                Style="max-width: 500px">
                                <Columns>
                                    <asp:BoundField DataField="Id" HeaderText="Id" />
                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Country" HeaderText="Country" />
                                    <asp:BoundField DataField="Salary" HeaderText="Salary" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="Edit" CommandArgument='<%# Eval("Id") %>'
                                                class="btn btn-primary" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" OnClick="Delete" CommandArgument='<%# Eval("Id") %>'
                                                class="btn btn-danger" />
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
                                                <b>Name</b>
                                            </th>
                                            <th class="hidden-xs">
                                                <b>Country</b>
                                            </th>
                                            <th class="hidden-xs">
                                                <b>Salary</b>
                                            </th>
                                            <th>
                                                &nbsp;
                                            </th>
                                            <th>
                                                &nbsp;
                                            </th>
                                        </tr>
                                        <tr>
                                            <td colspan="6" align="center" style="text-align: center;">
                                                <b>No Records Found</b>
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:Button ID="btnAdd" runat="server" Text="Add New Employee" OnClick="Add" class="btn btn-success" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
 
    <%--lnkFake Link Button for mpeAddUpdateEmployee ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkFake" runat="server"></asp:LinkButton>
 
    <%--pnlAddUpdateEmployeeDetails Panel With Design--%>
    <asp:Panel ID="pnlAddUpdateEmployeeDetails" runat="server" CssClass="modalPopup"
        Style="display: none;">
        <div style="overflow-y: auto; overflow-x: hidden; max-height: 450px;">
            <div class="modal-header">
                <asp:Label ID="lblHeading" runat="server" CssClass="modal-title"></asp:Label>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-3">
                            <label for="txtName">
                                Name</label>
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Name"
                                Width="150px"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:RequiredFieldValidator ID="rfvName" Display="Dynamic" ValidationGroup="Employee"
                                ErrorMessage="Required" ControlToValidate="txtName" runat="server" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-3">
                            <label for="txtCountry">
                                Country</label>
                        </div>
                        <div class="col-md-6">
                            <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" placeholder="Country"
                                Width="150px"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <asp:RequiredFieldValidator ID="rfvCountry" ErrorMessage="Required" Display="Dynamic"
                                ValidationGroup="Employee" ControlToValidate="txtCountry" runat="server" ForeColor="Red" />
                        </div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="row">
                        <div class="col-md-3">
                            <label for="ddlSalary">
                                Salary</label>
                        </div>
                        <div class="col-md-6">
                            <asp:DropDownList ID="ddlSalary" runat="server" CssClass="form-control" placeholder="Salary"
                                Width="150px"></asp:DropDownList>
<%--                            <asp:TextBox ID="txtSalary" runat="server" CssClass="form-control" placeholder="Salary"
                                Width="150px" ></asp:TextBox>--%>
                            <%--<uc:FilteredTextBoxExtender ID="ftSalary" runat="server" TargetControlID="txtSalary"
                                FilterType="Custom" ValidChars="1234567890.">
                            </uc:FilteredTextBoxExtender>--%>
                        </div>
                        <div class="col-md-3">
                            <%--<asp:RequiredFieldValidator ID="rfvSalary" Display="Dynamic" ValidationGroup="Employee"
                                ErrorMessage="Required" ControlToValidate="txtSalary" runat="server" ForeColor="Red" />--%>
                        </div>
                    </div>
                </div>
            </div>
            <div align="center" class="modal-footer">
                <div class="row">
                    <div class="col-md-12">
                        <asp:HiddenField ID="hfAddEditEmployeeId" runat="server" Value="0" />
                        <asp:HiddenField ID="hfAddEdit" runat="server" Value="ADD" />
                        <asp:Button ID="btnSave" runat="server" Text="ADD" OnClick="Save" class="btn btn-success"
                            ValidationGroup="Employee"></asp:Button>
                        <button id="btnCancel" runat="server" class="btn btn-primary">
                            Cancel
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
 
     <%--mpeAddUpdateEmployee Modal Popup Extender For pnlAddUpdateEmployeeDetails--%>
    <uc:ModalPopupExtender ID="mpeAddUpdateEmployee" runat="server" PopupControlID="pnlAddUpdateEmployeeDetails"
        TargetControlID="lnkFake" BehaviorID="mpeAddUpdateEmployee" CancelControlID="btnCancel"
        BackgroundCssClass="modalBackground">
    </uc:ModalPopupExtender>
 
    <%--lnkFake1 Link Button for mpeDeleteEmployee ModalPopup as TargetControlID--%>
    <asp:LinkButton ID="lnkFake1" runat="server"></asp:LinkButton>
 
    <%--pnlDeleteEmployee Panel With Design--%>
    <asp:Panel ID="pnlDeleteEmployee" runat="server" CssClass="modalPopup" Style="display: none;">
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
                    <asp:HiddenField ID="hfDeleteEmployeeId" runat="server" Value="0" />
                    <asp:Button ID="btnYes" runat="server" Text="Yes" OnClick="Yes" class="btn btn-danger">
                    </asp:Button>
                    <button id="btnNo" runat="server" class="btn btn-default">
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </asp:Panel>
 
   <%-- mpeDeleteEmployee Modal Popup Extender For pnlDeleteEmployee--%>
    <uc:ModalPopupExtender ID="mpeDeleteEmployee" runat="server" PopupControlID="pnlDeleteEmployee"
        TargetControlID="lnkFake1" BehaviorID="mpeDeleteEmployee" CancelControlID="btnNo"
        BackgroundCssClass="modalBackground">
    </uc:ModalPopupExtender>



        <asp:SqlDataSource ID="DSSalaries" runat="server" 
            ConnectionString="<%$ ConnectionStrings:appmgrConnectionString %>"
            SelectCommand = "SELECT * FROM TestSalary">
        </asp:SqlDataSource>
</asp:Content>
