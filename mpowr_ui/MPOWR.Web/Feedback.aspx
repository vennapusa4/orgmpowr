<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Feedback.aspx.cs" Inherits="MPOWR.Web.components.feedback.Feedback" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <div class="dashboard" >
    
    
    <div class="col-lg-offset-1 col-md-offset-1 col-sm-offset-1 col-xs-offset-1 col-lg-9 col-md-9 col-sm-9 col-xs-9 feedback-form" >
        <div class="header">
            <h4>Feedback / Suggestion:</h4>
        </div>
        <!--<div class="frm-ss">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <label>Screen Shot</label>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6"> </div>
        </div>--> 
        <div class="frm-sm-id">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <label>TO</label>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                 <asp:TextBox ID="txtTo" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="frm-scr-module">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <label>Subject</label>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
               <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="frm-comments">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <label>Body</label>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                 <asp:TextBox ID="txtBody" runat="server" TextMode = "MultiLine"  Height = "150" Width = "500"></asp:TextBox>
            </div>
        </div>
        <div class="frm-scr-module">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <label>Attachments</label>
            </div>
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
              <asp:FileUpload ID="fuAttachment" runat="server" />
            </div>
        </div>
        <div class="frm-btns">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12 pull-right">
                 <asp:Button CssClass="btn btn-green btn-cancel" Text="Send" OnClick="Unnamed_Click"   runat="server" />
             
                
            </div>
        </div>
    </div>

</div>
    </form>
</body>
</html>
