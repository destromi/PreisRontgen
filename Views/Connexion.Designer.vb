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
        TextBox2 = New TextBox()
        Label2 = New Label()
        TextBox1 = New TextBox()
        Label1 = New Label()
        Label3 = New Label()
        Button1 = New Button()
        SuspendLayout()
        ' 
        ' Label4
        ' 
        Label4.AutoSize = True
        Label4.ForeColor = Color.LimeGreen
        Label4.Location = New Point(168, 89)
        Label4.Name = "Label4"
        Label4.Size = New Size(217, 20)
        Label4.TabIndex = 5
        Label4.Text = "Suivi des prix et hausse des prix"
        ' 
        ' Label1Titre
        ' 
        Label1Titre.AutoSize = True
        Label1Titre.Font = New Font("Times New Roman", 36F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label1Titre.ForeColor = Color.LimeGreen
        Label1Titre.Location = New Point(161, 29)
        Label1Titre.Name = "Label1Titre"
        Label1Titre.Size = New Size(230, 68)
        Label1Titre.TabIndex = 4
        Label1Titre.Text = "Pricieux"
        ' 
        ' TextBox2
        ' 
        TextBox2.Location = New Point(287, 207)
        TextBox2.Name = "TextBox2"
        TextBox2.Size = New Size(239, 27)
        TextBox2.TabIndex = 8
        TextBox2.UseSystemPasswordChar = True
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Font = New Font("Bookman Old Style", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Label2.Location = New Point(23, 209)
        Label2.Name = "Label2"
        Label2.Size = New Size(132, 21)
        Label2.TabIndex = 6
        Label2.Text = "Mot de passe :"
        ' 
        ' TextBox1
        ' 
        TextBox1.Location = New Point(287, 152)
        TextBox1.Name = "TextBox1"
        TextBox1.Size = New Size(239, 27)
        TextBox1.TabIndex = 9
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Font = New Font("Bookman Old Style", 10F)
        Label1.Location = New Point(23, 158)
        Label1.Name = "Label1"
        Label1.Size = New Size(166, 21)
        Label1.TabIndex = 7
        Label1.Text = "Nom d'utilisateur :"
        ' 
        ' Label3
        ' 
        Label3.AutoSize = True
        Label3.Location = New Point(276, 344)
        Label3.Name = "Label3"
        Label3.Size = New Size(0, 20)
        Label3.TabIndex = 10
        ' 
        ' Button1
        ' 
        Button1.BackColor = Color.LimeGreen
        Button1.Font = New Font("Bookman Old Style", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Button1.ForeColor = Color.White
        Button1.Location = New Point(171, 271)
        Button1.Name = "Button1"
        Button1.Size = New Size(210, 47)
        Button1.TabIndex = 11
        Button1.Text = "Se connecter"
        Button1.UseVisualStyleBackColor = False
        ' 
        ' Connexion
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(552, 378)
        Controls.Add(Button1)
        Controls.Add(Label3)
        Controls.Add(TextBox2)
        Controls.Add(Label2)
        Controls.Add(TextBox1)
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
    Friend WithEvents TextBox2 As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Button1 As Button

End Class
