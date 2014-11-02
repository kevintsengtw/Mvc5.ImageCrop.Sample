<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ImageCrop.WebForm._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

	<h2>ImageCrop.WebForm - Default</h2>
	<p>
		<a href="Upload.aspx" class="btn btn-primary">上傳圖片</a>
	</p>
	<hr />
	<asp:ListView ID="ListView1" runat="server" onitemdatabound="ListView1_ItemDataBound" onitemcommand="ListView1_ItemCommand">
		<LayoutTemplate>
			<table id="itemPlaceholderContainer" runat="server" class="table table-bordered table-striped table-condensed">
				<tr style="background-color: #aaaaaa; text-align: center; font-weight: bold; height: 40px;">
					<td>
						Option
					</td>
					<td>
						Original Imgae
					</td>
					<td>
						Crop Image
					</td>
					<td>
						Create Date <br />
						Update Date
					</td>
				</tr>
				<tr id="itemPlaceholder" runat="server">
				</tr>
			</table>
		</LayoutTemplate>
		<AlternatingItemTemplate>
			<tr style="background-color: #dddddd; text-align: center;">
				<td>
					<strong><%# Eval("ID") %></strong>
					<br />
					<asp:HyperLink ID="HyperLink1" runat="server">Crop</asp:HyperLink>
					<asp:LinkButton ID="LinkButton1" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>' onclientclick="return confirm('你確定要刪除這筆資料嗎？')">Delete</asp:LinkButton>
				</td>
				<td>
					<asp:Image ID="Image1" runat="server" />
					<br />
					<%# Eval("OriginalImage") %>
				</td>
				<td>
					<asp:Image ID="Image2" runat="server" />
					<br />
					<%# Eval("CropImage") %>
				</td>
				<td>
					<%# Eval("CreateDate", "{0:yyyy-MM-dd HH:mm:ss}") %>
					<br />
					<%# Eval("UpdateDate", "{0:yyyy-MM-dd HH:mm:ss}")%>
				</td>
			</tr>		
		</AlternatingItemTemplate>
		<itemtemplate>
			<tr style="background-color: #ffffff; text-align: center;">
				<td>
					<strong><%# Eval("ID") %></strong>
					<br />
					<asp:HyperLink ID="HyperLink1" runat="server">Crop</asp:HyperLink>
					<asp:LinkButton ID="LinkButton1" runat="server" CommandName="DeleteItem" CommandArgument='<%# Eval("ID") %>' onclientclick="return confirm('你確定要刪除這筆資料嗎？')">Delete</asp:LinkButton>
				</td>
				<td>
					<asp:Image ID="Image1" runat="server" />
					<br />
					<%# Eval("OriginalImage") %>
				</td>
				<td>
					<asp:Image ID="Image2" runat="server" />
					<br />
					<%# Eval("CropImage") %>
				</td>
				<td>
					<%# Eval("CreateDate", "{0:yyyy-MM-dd HH:mm:ss}") %>
					<br />
					<%# Eval("UpdateDate", "{0:yyyy-MM-dd HH:mm:ss}")%>
				</td>
			</tr>
		</itemtemplate>
	</asp:ListView>

</asp:Content>
