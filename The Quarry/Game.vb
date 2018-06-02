Public Class Game
    'Dominick Manicone and Brandon Robayo


    'Objects
    Private Player As New PictureBox
    Private Crystal As New PictureBox



    Private Sub Game_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Constants
        Const xMapSize As Integer = 1024
        Const yMapSize As Integer = 1024

        'Initializing form
        Me.ClientSize = New Size(xMapSize, yMapSize)
        Me.Name = "The Quarry"

        'Create Objects
        'Player
        With Player
            .Size = New Size(32, 32)
            .Name = "Player"
            .Visible = True
            .BackColor = Color.Red
            .Location = New Point(xMapSize / 2 - 16, yMapSize / 2 + 48)
            Controls.Add(Player)
        End With

        'Crystal
        With Crystal
            .Size = New Size(64, 32)
            .Name = "Crystal"
            .Location = New Point(xMapSize / 2 - 32, yMapSize / 2 - 16)
            .Image = My.Resources.Crystal
            .Visible = True
            Controls.Add(Crystal)
        End With




    End Sub

    Private Sub Game_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyCode
            Case Keys.W
                Player.Tag = "u"
            Case Keys.A
                Player.Tag = "l"
            Case Keys.S
                Player.Tag = "d"
            Case Keys.D
                Player.Tag = "r"
            Case Else
                'do nothing, we are not moving.
        End Select
    End Sub

    Private Sub Movement(ByRef obj As PictureBox, ByVal Speed As Integer)
        With obj
            Select Case .Tag
                Case "r"
                    .Left += Speed
                Case "l"
                    .Left -= Speed
                Case "u"
                    .Top -= Speed
                Case "d"
                    .Top += Speed
                Case Else
                    'do nothing, we are not moving.
            End Select
        End With

    End Sub

    Sub Collision(ByRef obj As PictureBox, ByRef Player As PictureBox)
        'Collisions with objects
        With Player
            If .Bounds.IntersectsWith(obj.Bounds) Then
                If .Tag = "r" Then
                    .Left = obj.Left - .Width
                ElseIf .Tag = "l" Then
                    .Left = obj.Right
                ElseIf .Tag = "u" Then
                    .Top = obj.Bottom
                Else
                    .Top = obj.Top - .Height
                End If
            End If
        End With
    End Sub

    Private Sub tmrMovement_Tick(sender As Object, e As EventArgs) Handles tmrMovement.Tick
        Dim mpos As Point = PointToClient(MousePosition)
        Dim dir As Integer = point_at(Player.Location, mpos)
        Movement(Player, Chars.Player.Speed)
    End Sub

    Private Sub Game_KeyUp(sender As Object, e As KeyEventArgs) Handles Me.KeyUp
        Player.Tag = ""
    End Sub

    Private Function point_at(ByVal pos1 As Point, ByVal pos2 As Point) As Integer 'takes in an x & y to point towards the cursor from said position.
        Dim dir As Integer, rad As Double
        'd stands for delta.         
        Dim dX As Integer = pos2.X - pos1.X
        Dim dY As Integer = pos2.Y - pos1.Y
        rad = Math.Atan2(dY, dX)
        dir = rad * (180 / Math.PI)
        Return dir
    End Function

End Class

Public Class Chars

    Public Structure Player
        Public Const Speed As Integer = 5
    End Structure

End Class
