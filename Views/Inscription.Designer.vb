<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
<Global.System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726")>
Partial Class Inscription
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Label1Titre = New Label()
        Label1 = New Label()
        TextBoxUser = New TextBox()
        Label2 = New Label()
        TextBoxPwd = New TextBox()
        Label3 = New Label()
        TextBoxPwdC = New TextBox()
        Label4 = New Label()
        CheckBox1 = New CheckBox()
        BtnInscrire = New Button()
        LabelConnecter = New Label()
        Label6 = New Label()
        LabelError = New Label()
        SuspendLayout()
        ' 
        ' Label1Titre
        ' 
        Label1Titre.AutoSize = True
        Label1Titre.Font = New Font("Times New Roman", 36F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1Titre.ForeColor = Color.LimeGreen
        Label1Titre.Location = New Point(161, 23)
        Label1Titre.Name = "Label1Titre"
        Label1Titre.Size = New Size(230, 68)
        Label1Titre.TabIndex = 0
        Label1Titre.Text = "Pricieux"
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Bookman Old Style", 10F)
        Label1.Location = New Point(25, 150)
        Label1.Name = "Label1"
        Label1.Size = New Size(166, 21)
        Label1.TabIndex = 1
        Label1.Text = "Nom d'utilisateur :"
        ' 
        ' TextBoxUser
        ' 
        TextBoxUser.Font = New Font("Bookman Old Style", 10.2F)
        TextBoxUser.Location = New Point(289, 144)
        TextBoxUser.Name = "TextBoxUser"
        TextBoxUser.Size = New Size(239, 27)
        TextBoxUser.TabIndex = 2
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(25, 201)
        Label2.Name = "Label2"
        Label2.Size = New Size(132, 21)
        Label2.TabIndex = 1
        Label2.Text = "Mot de passe :"
        ' 
        ' TextBoxPwd
        ' 
        TextBoxPwd.Font = New Font("Bookman Old Style", 10.2F)
        TextBoxPwd.Location = New Point(289, 199)
        TextBoxPwd.Name = "TextBoxPwd"
        TextBoxPwd.Size = New Size(239, 27)
        TextBoxPwd.TabIndex = 2
        TextBoxPwd.UseSystemPasswordChar = True
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label3.Location = New Point(25, 255)
        Label3.Name = "Label3"
        Label3.Size = New Size(241, 21)
        Label3.TabIndex = 1
        Label3.Text = "Confirmer le mot de passe :"
        ' 
        ' TextBoxPwdC
        ' 
        TextBoxPwdC.Font = New Font("Bookman Old Style", 10.2F)
        TextBoxPwdC.Location = New Point(289, 249)
        TextBoxPwdC.Name = "TextBoxPwdC"
        TextBoxPwdC.Size = New Size(239, 27)
        TextBoxPwdC.TabIndex = 2
        TextBoxPwdC.UseSystemPasswordChar = True
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.ForeColor = Color.LimeGreen
        Label4.Location = New Point(166, 83)
        Label4.Name = "Label4"
        Label4.Size = New Size(221, 20)
        Label4.TabIndex = 3
        Label4.Text = "Suivi des prix et hausse des prix "
        ' 
        ' CheckBox1
        ' 
        CheckBox1.AutoSize = True
        CheckBox1.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        CheckBox1.Location = New Point(25, 322)
        CheckBox1.Name = "CheckBox1"
        CheckBox1.Size = New Size(441, 25)
        CheckBox1.TabIndex = 4
        CheckBox1.Text = "Acceptez les conditions d'utilisations de Pricieux"
        CheckBox1.UseVisualStyleBackColor = True
        ' 
        ' BtnInscrire
        ' 
        BtnInscrire.BackColor = Color.LimeGreen
        BtnInscrire.Font = New Font("Bookman Old Style", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        BtnInscrire.ForeColor = Color.White
        BtnInscrire.Location = New Point(198, 376)
        BtnInscrire.Name = "BtnInscrire"
        BtnInscrire.Size = New Size(150, 50)
        BtnInscrire.TabIndex = 5
        BtnInscrire.Text = "S'inscrire"
        BtnInscrire.UseVisualStyleBackColor = False
        ' 
        ' LabelConnecter
        ' 
        LabelConnecter.AutoSize = True
        LabelConnecter.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        LabelConnecter.ForeColor = Color.RoyalBlue
        LabelConnecter.Location = New Point(212, 436)
        LabelConnecter.Name = "LabelConnecter"
        LabelConnecter.Size = New Size(119, 21)
        LabelConnecter.TabIndex = 6
        LabelConnecter.Text = "Se connecter"
        ' 
        ' Label6
        ' 
        Label6.AutoSize = True
        Label6.Location = New Point(155, 463)
        Label6.Name = "Label6"
        Label6.Size = New Size(0, 20)
        Label6.TabIndex = 7
        ' 
        ' LabelError
        ' 
        LabelError.AutoSize = True
        LabelError.ForeColor = Color.Red
        LabelError.Location = New Point(118, 289)
        LabelError.Name = "LabelError"
        LabelError.Size = New Size(0, 20)
        LabelError.TabIndex = 8
        ' 
        ' Inscription
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(552, 496)
        Controls.Add(LabelError)
        Controls.Add(Label6)
        Controls.Add(LabelConnecter)
        Controls.Add(BtnInscrire)
        Controls.Add(CheckBox1)
        Controls.Add(Label4)
        Controls.Add(TextBoxPwdC)
        Controls.Add(Label3)
        Controls.Add(TextBoxPwd)
        Controls.Add(Label2)
        Controls.Add(TextBoxUser)
        Controls.Add(Label1)
        Controls.Add(Label1Titre)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "Inscription"
        SizeGripStyle = SizeGripStyle.Hide
        StartPosition = FormStartPosition.CenterParent
        Text = "Inscription"
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents Label1Titre As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents TextBoxUser As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBoxPwd As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents TextBoxPwdC As TextBox
    Friend WithEvents Label4 As Label

    Public Sub New()
        Text = "Precieux - Inscription"
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub

    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents BtnInscrire As Button
    Friend WithEvents LabelConnecter As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents LabelError As Label
End Class
