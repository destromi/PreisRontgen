<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")> _
Partial Class Connexion
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Label4 = New Label()
        Label1Titre = New Label()
        TextBoxPwd = New TextBox()
        Label2 = New Label()
        TextBoxUser = New TextBox()
        Label1 = New Label()
        LabelError = New Label()
        BtnConnect = New Button()
        LabelInscrire = New Label()
        SuspendLayout()
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.ForeColor = Color.ForestGreen
        Label4.Location = New Point(168, 78)
        Label4.Name = "Label4"
        Label4.Size = New Size(217, 20)
        Label4.TabIndex = 5
        Label4.Text = "Suivi des prix et hausse des prix"
        ' 
        ' Label1Titre
        ' 
        Label1Titre.AutoSize = True
        Label1Titre.Font = New Font("Times New Roman", 36F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1Titre.ForeColor = Color.ForestGreen
        Label1Titre.Location = New Point(161, 18)
        Label1Titre.Name = "Label1Titre"
        Label1Titre.Size = New Size(230, 68)
        Label1Titre.TabIndex = 4
        Label1Titre.Text = "Pricieux"
        ' 
        ' TextBoxPwd
        ' 
        TextBoxPwd.Location = New Point(289, 188)
        TextBoxPwd.Name = "TextBoxPwd"
        TextBoxPwd.Size = New Size(239, 27)
        TextBoxPwd.TabIndex = 8
        TextBoxPwd.UseSystemPasswordChar = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(25, 190)
        Label2.Name = "Label2"
        Label2.Size = New Size(132, 21)
        Label2.TabIndex = 6
        Label2.Text = "Mot de passe :"
        ' 
        ' TextBoxUser
        ' 
        TextBoxUser.Location = New Point(289, 133)
        TextBoxUser.Name = "TextBoxUser"
        TextBoxUser.Size = New Size(239, 27)
        TextBoxUser.TabIndex = 9
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Bookman Old Style", 10F)
        Label1.Location = New Point(25, 139)
        Label1.Name = "Label1"
        Label1.Size = New Size(166, 21)
        Label1.TabIndex = 7
        Label1.Text = "Nom d'utilisateur :"
        ' 
        ' LabelError
        ' 
        LabelError.AutoSize = True
        LabelError.ForeColor = Color.Red
        LabelError.Location = New Point(242, 238)
        LabelError.Name = "LabelError"
        LabelError.Size = New Size(0, 20)
        LabelError.TabIndex = 10
        ' 
        ' BtnConnect
        ' 
        BtnConnect.BackColor = Color.LimeGreen
        BtnConnect.Font = New Font("Bookman Old Style", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnConnect.ForeColor = Color.White
        BtnConnect.Location = New Point(161, 273)
        BtnConnect.Name = "BtnConnect"
        BtnConnect.Size = New Size(210, 47)
        BtnConnect.TabIndex = 11
        BtnConnect.Text = "Se connecter"
        BtnConnect.UseVisualStyleBackColor = False
        ' 
        ' LabelInscrire
        ' 
        LabelInscrire.AutoSize = True
        LabelInscrire.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        LabelInscrire.ForeColor = Color.RoyalBlue
        LabelInscrire.Location = New Point(232, 331)
        LabelInscrire.Name = "LabelInscrire"
        LabelInscrire.Size = New Size(88, 21)
        LabelInscrire.TabIndex = 12
        LabelInscrire.Text = "S'inscrire"
        ' 
        ' Connexion
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(552, 378)
        Controls.Add(LabelInscrire)
        Controls.Add(BtnConnect)
        Controls.Add(LabelError)
        Controls.Add(TextBoxPwd)
        Controls.Add(Label2)
        Controls.Add(TextBoxUser)
        Controls.Add(Label1)
        Controls.Add(Label4)
        Controls.Add(Label1Titre)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "Connexion"
        SizeGripStyle = SizeGripStyle.Hide
        StartPosition = FormStartPosition.CenterParent
        Text = "Connexion"
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents Label4 As Label
    Friend WithEvents Label1Titre As Label
    Friend WithEvents TextBoxPwd As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxUser As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents LabelError As Label
    Friend WithEvents BtnConnect As Button
    Friend WithEvents LabelInscrire As Label

End Class
