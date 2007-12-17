<%@ Assembly Name="Microsoft.SharePoint.ApplicationPages, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c"%> <%@ Page Language="C#" Inherits="Microsoft.SharePoint.ApplicationPages.SearchResultsPage" MasterPageFile="~/_layouts/application.master"   EnableViewState="false" EnableViewStateMac="false"   %>
<%@ Import Namespace="Microsoft.SharePoint.ApplicationPages" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register Tagprefix="wssawc" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="SearchWC" Namespace="Microsoft.SharePoint.Search.Internal.WebControls" Assembly="Microsoft.SharePoint.Search, Version=12.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<%@ Register Tagprefix="GooG" Namespace="GoogleyControl" Assembly="GoogleyControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=d9bc894ba8f8b12e" %>


<asp:Content ContentPlaceHolderId="PlaceHolderPageTitle" runat="server">
	<SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,searchresults_pagetitle%>" EncodeMethod='HtmlEncode'/>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">
<style type="text/css">
.ms-titlearea
{
	padding-top: 0px !important;
}
.ms-areaseparatorright {
	PADDING-RIGHT: 6px;
}
td.ms-areaseparatorleft{
	border-right:0px;
}
div.ms-areaseparatorright{
	border-left:0px !important;
}
</style>
<script language="javascript">
	function _spFormOnSubmit()
	{
		return GoSearch();
	}
	function SetPageTitle()
	{
	   var Query = "";
	   if (window.top.location.search != 0)
	   {
	    
		  Query = window.top.location.search;
		  var keywordQuery = getParameter(Query, 'k');
		  if(keywordQuery != null)
		  {
			 var titlePrefix = '<asp:Literal runat="server" text="<%$Resources:wss,searchresults_pagetitle%>"/>';
			 document.title = titlePrefix + ": " +keywordQuery;
		  }
	   }
	}
	function getParameter (queryString, parameterName)
	{
	   var parameterName = parameterName + "=";
	   if (queryString.length > 0)
	   {
		 var begin = queryString.indexOf (parameterName);
		 if (begin != -1)
		 {
			begin += parameterName.length;
			var end = queryString.indexOf ("&" , begin);
			if (end == -1)
			{
			   end = queryString.length;
			}
			return decodeURIComponent(queryString.substring (begin, end));
		 }
	   }
	   return null;
	}
if (document.addEventListener)
{
	document.addEventListener("DOMContentLoaded", SetPageTitle, false);
}
else if(document.attachEvent)
{
	document.attachEvent("onreadystatechange", SetPageTitle);
}
</script>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderTitleAreaClass" runat="server">
ms-searchresultsareaSeparator
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderNavSpacer" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="PlaceHolderTitleBreadcrumb"  runat="server">
<A name="mainContent"></A>
	<TABLE width="100%" cellpadding="2" cellspacing="0" border="0">
	 <tr>
	  <td height="5"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></td>
	 </tr>
	 <tr>
	  <td valign="top" class="ms-descriptiontext" style="padding-bottom: 5px">
	   <b>
		<label for=<%SPHttpUtility.AddQuote(SPHttpUtility.NoEncode(SearchString.ClientID),Response.Output);%> >
		 <SharePoint:EncodedLiteral runat="server" text="<%$Resources:wss,searchresults_searchforitems%>" EncodeMethod='HtmlEncode'/>
		 
		 
		 
		</label>
	   </b>
	  </td>
	 </tr>
	 <tr>
	  <td class="ms-vb">
		<table border=0 cellpadding="0" cellspacing="0">
		 <tr>
		  <td>
		   <asp:DropDownList ID="SearchScope" class="ms-searchbox" Tooltip="<%$Resources:wss,search_searchscope%>" runat="server"/>
		  </td>
		  <td>
		   <asp:TextBox ID="SearchString" Columns="40" class="ms-searchbox" AccessKey=S MaxLength=255 Tooltip="<%$Resources:wss,searchresults_SearchBoxToolTip%>" runat="server"/>
		  </td>
		  <td valign="center"><div class="ms-searchimage" style="padding-bottom:3px">
		   <asp:ImageButton ID="ImgGoSearch" BorderWidth=0 AlternateText="<%$Resources:wss,searchresults_AlternateText%>" ImageUrl="/_layouts/images/gosearch.gif" runat="server"/></div>
		  </td>
		 </tr>
		</table>
	   </td>
	 </tr>
	 <tr>
	  <td height="10" colspan="8"><IMG SRC="/_layouts/images/blank.gif" width=1 height=1 alt=""></td>
	 </tr>
	</TABLE>
</asp:Content>
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">

	<asp:PlaceHolder Runat="server" ID="SearchSummary">
	  <TABLE id="SearchSummaryTable" width="100%" cellpadding=0 cellspacing=0 border=0>
		<TR>
		
		 <TD id="UpperLeftCell" align="left">
		  <SearchWC:SearchSummaryWebPart runat="server" FrameType="None"/>
		 </TD>
		 
		 <TD id="UpperRightCell" align="right">
		 </TD>
		</TR>
		<TR>
		
		 <TD id="MidRightCell" align="right">
					<SearchWC:SearchPagingWebPart runat="server" FrameType="None"/>
		 </TD>
		</TR>



<!-- START GOOGLE ADDITION:  Xen Lategan 24/11/2007 -->
		</TABLE>
		<TR>
		 <TD id="LowerCell" colspan=2>
		   <GooG:GSAPart ID="GSAPart1" runat="server"/>
		   <%
		       GoogleyControl.GSAPart m = new GSAPart();
		       Response.Write(m.GSASearch(Request.QueryString)); 
		   %> 
		   <hr>
		   
		  <!--  Disable these to lines to exlude Sharepoint results -->	
		  <SearchWC:SearchStatsWebPart ID="SearchStatsWebPart1" runat="server" FrameType="None"/>
   		  <SearchWC:CoreResultsWebPart runat="server" FrameType="None"/>
   		  <!-- -->
   			
		 </TD>
		</TR>
		<TABLE id="SearchSummaryTable" width="100%" cellpadding=0 cellspacing=0 border=0>
<!-- END GOOGLE ADDITION:  Xen Lategan 24/11/2007 -->

		<TR>
		 <TD id="LowerCell" colspan=2>
		    <SearchWC:SearchPagingWebPart runat="server" FrameType="None"/>
		 </TD>
		</TR>
	  </TABLE>

	</asp:PlaceHolder>
</asp:Content>
