Option Strict On
Option Explicit On

Imports System.Windows.Forms

Friend Module Program
    <STAThread>
    Friend Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)
        If Not Configuration.ConnexionConfiguree() Then
            Using formulaire As New ConfigurationBaseForm()
                If formulaire.ShowDialog() <> DialogResult.OK Then Return
            End Using
        End If
        Application.Run(New LoginForm(New AuthentificationDepot()))
    End Sub
End Module
