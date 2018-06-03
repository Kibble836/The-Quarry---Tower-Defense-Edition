Public Class Game
    'Dominick Manicone and Brandon Robayo


    'Objects
    Private Player As New PictureBox
    Private Crystal As New PictureBox
    Private Colliders(19) As PictureBox



    Private Sub Game_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Constants
        Const xMapSize As Integer = 768
        Const yMapSize As Integer = 768
        Const TileSize As Integer = 32

        'Initializing form
        Me.ClientSize = New Size(xMapSize, yMapSize)
        Me.Name = "The Quarry"
        Me.BackgroundImage = My.Resources.background

        'Create Objects
        'Player
        With Player
            .Size = New Size(TileSize, TileSize)
            .Name = "Player"
            .Visible = True
            .BackColor = Color.Red
            .Location = New Point(xMapSize / 2 - 16, yMapSize / 2 + 48)
            Controls.Add(Player)
        End With

        'Crystal
        With Crystal
            .Size = New Size(2 * TileSize, TileSize)
            .Name = "Crystal"
            .BackColor = Color.Transparent
            .Location = New Point(xMapSize / 2 - TileSize, yMapSize / 2 - TileSize)
            .Image = My.Resources.Crystal
            .Visible = True
            Controls.Add(Crystal)
        End With

        'Colliders
        For i As Integer = 0 To 19
            Colliders(i) = New PictureBox
            With Colliders(i)
                .Visible = False
                Select Case i
                    Case 0
                        .Location = New Point(0 * TileSize, 0 * TileSize)
                        .Size = New Size(8 * TileSize, 2 * TileSize)
                    Case 1
                        .Location = New Point(0 * TileSize, 2 * TileSize)
                        .Size = New Size(2 * TileSize, 6 * TileSize)
                    Case 2
                        .Location = New Point(8 * TileSize, 0 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 3
                        .Location = New Point(0 * TileSize, 8 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 4
                        .Location = New Point(2 * TileSize, 2 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)

                    Case 5
                        .Location = New Point(16 * TileSize, 0 * TileSize)
                        .Size = New Size(8 * TileSize, 2 * TileSize)
                    Case 6
                        .Location = New Point(22 * TileSize, 2 * TileSize)
                        .Size = New Size(2 * TileSize, 6 * TileSize)
                    Case 7
                        .Location = New Point(15 * TileSize, 0 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 8
                        .Location = New Point(23 * TileSize, 8 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 9
                        .Location = New Point(21 * TileSize, 2 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)

                    Case 10
                        .Location = New Point(2 * TileSize, 22 * TileSize)
                        .Size = New Size(6 * TileSize, 2 * TileSize)
                    Case 11
                        .Location = New Point(0 * TileSize, 16 * TileSize)
                        .Size = New Size(2 * TileSize, 8 * TileSize)
                    Case 12
                        .Location = New Point(8 * TileSize, 23 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 13
                        .Location = New Point(0 * TileSize, 15 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 14
                        .Location = New Point(2 * TileSize, 21 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)

                    Case 15
                        .Location = New Point(16 * TileSize, 22 * TileSize)
                        .Size = New Size(8 * TileSize, 2 * TileSize)
                    Case 16
                        .Location = New Point(22 * TileSize, 16 * TileSize)
                        .Size = New Size(2 * TileSize, 6 * TileSize)
                    Case 17
                        .Location = New Point(15 * TileSize, 23 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 18
                        .Location = New Point(23 * TileSize, 15 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                    Case 19
                        .Location = New Point(21 * TileSize, 21 * TileSize)
                        .Size = New Size(1 * TileSize, 1 * TileSize)
                End Select
                Controls.Add(Colliders(i))
            End With

        Next




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
        For Each obj In Colliders
            Collision(obj, Player)
        Next

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
