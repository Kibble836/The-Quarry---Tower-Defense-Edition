Public Class Game
    'Dominick Manicone and Brandon Robayo

    'Objects
    Private Player As New PictureBox
    Private Crystal As New PictureBox
    Private Boss = New PictureBox
    Private Colliders(19) As PictureBox

    Public Const xMapSize As Integer = 768
    Public Const yMapSize As Integer = 768

    Private Spawnlocations(3) As Point

    Private PHealth As Integer = PStats.MaxHealth
    Private CHealth As Integer = 1000


    Private blnGameOver As Boolean = False

    Private bossNum As Integer = 0

    Private CStats
    Private PStats As Chars.Player
    Private RedStats As Chars.RedStats
    Private GreenStats As Chars.GreenStats
    Private BlueStats As Chars.BlueStats
    Private PurpleStats As Chars.PurpleStats
    Private TransStats As Chars.TransStats



    Private Sub DrawHealthbar(ByVal Percent As Double, ByVal LLeft As Point, ByVal URight As Point, ByVal fcolor As Brush)
        'initialize pens
        Dim bpen As New Pen(Brushes.Gray)
        Dim fpen As New Pen(fcolor)
        Dim epen As New Pen(Color.Black)
        'background dimensions
        Dim bheight As Integer = URight.Y - LLeft.Y
        Dim bwidth As Integer = URight.X - LLeft.X
        Dim fheight As Integer = bheight / 3
        'misc values
        Dim startY As Integer = LLeft.Y - bheight / 2
        Dim length As Integer = Math.Abs((URight.X - LLeft.X) * Percent)
        'setting pen width
        bpen.Width = bheight
        epen.Width = fheight
        fpen.Width = fheight
        'drawing lines
        Me.CreateGraphics.Flush()
        Me.CreateGraphics.DrawLine(bpen, LLeft.X, startY, URight.X, startY) 'background
        Me.CreateGraphics.DrawLine(epen, LLeft.X, startY, URight.X, startY) 'empty central bar
        Me.CreateGraphics.DrawLine(fpen, LLeft.X, startY, LLeft.X + length, startY) 'value
    End Sub

    Private Sub SpawnBoss()
        With Boss

            .Location = Spawnlocations(Rand(0, 3))
            .Visible = True
            bossNum += 1
            Select Case bossNum
                Case 1
                    .BackColor = Color.Red
                    .tag = RedStats.MaxHealth
                    .Size = New Size(RedStats.Scale, RedStats.Scale)
                    CStats = RedStats
                Case 2
                    .BackColor = Color.Green
                    .tag = Greenstats.MaxHealth
                    .size = New Size(GreenStats.Scale, GreenStats.Scale)
                    CStats = GreenStats
                Case 3
                    .backcolor = Color.Blue
                    .tag = BlueStats.MaxHealth
                    .size = New Size(BlueStats.Scale, BlueStats.Scale)
                    CStats = BlueStats
                Case 4
                    .backcolor = Color.Purple
                    .tag = PurpleStats.MaxHealth
                    .size = New Size(PurpleStats.Scale, PurpleStats.Scale)
                    CStats = PurpleStats
                Case 5
                    .backcolor = Color.Transparent
                    .tag = TransStats.MaxHealth
                    .size = New Size(TransStats.Scale, TransStats.Scale)
                    CStats = TransStats
                Case Else
                    Boss.Dispose()
                    Controls.Remove(Boss)
            End Select
        End With

    End Sub

    Public Sub Movement(ByRef obj As PictureBox, ByVal Speed As Integer)
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

    Public Function Rand(ByVal intLow As Integer, ByVal intHigh As Integer) As Integer
        Return Int(Rnd() * (intHigh - intLow + 1) + intLow)
    End Function
    Public Function lengthdir_x(ByVal dist As Integer, ByVal dir As Integer) As Integer
        Return Math.Cos(toRad(dir)) * dist
    End Function
    Public Function lengthdir_y(ByVal dist As Integer, ByVal dir As Integer) As Integer
        Return Math.Sin(toRad(dir)) * dist
    End Function
    Public Function Clamp(ByVal dblVal As Double, ByVal dblMin As Double, ByVal dblMax As Double) 'Clamps a value between two numbers.
        If dblVal > dblMax Then
            dblVal = dblMax
        ElseIf dblVal < dblMin Then
            dblVal = dblMin
        End If
        Return dblVal
    End Function
    Private Function toRad(ByVal deg As Integer) As Double
        Return deg * (Math.PI / 180)
    End Function

    Public Sub Collision(ByRef obj As PictureBox, ByRef Player As PictureBox)
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

    Private Sub tmrMovement_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles tmrMovement.Tick
        Dim mpos As Point = PointToClient(MousePosition)
        Dim dir As Integer = point_at(Player.Location, mpos)
        Movement(Player, Chars.Player.Speed)
        Call DrawHealthbar(CHealth / 1000, New Point(10, 16), New Point(xMapSize - 10, 10), Brushes.YellowGreen)
        If bossNum = 1 Then
            RedStats.RedAI(Boss, Player)
        ElseIf bossNum = 2 Then
            GreenStats.GreenAI(Boss, Player)
        ElseIf bossNum = 3 Then
            BlueStats.BlueAI(Boss, Player)
        ElseIf bossNum = 4 Then
            PurpleStats.PurpleAI(Boss, Crystal)
        ElseIf bossNum = 5 Then
            TransStats.TransAI(Boss, Player)
        Else
            Boss.Dispose()
            Controls.Remove(Boss)
        End If

        If Boss.Tag <= 0 Then
            SpawnBoss()
        End If

        For Each obj In Colliders
            Collision(obj, Player)
        Next

        If Crystal.Bounds.IntersectsWith(Boss.Bounds) Then
            CHealth -= CStats.Damage
        End If
        CHealth = Clamp(CHealth, 0, 1000)
        If CHealth <= 0 And blnGameOver = False Then
            Crystal.Visible = False
            Boss.Dispose()
            Controls.Remove(Boss)
            blnGameOver = True
            Me.BackgroundImage = My.Resources.background_nocrystal
        End If
    End Sub

    Private Function collision_point(ByVal Colliders As PictureBox(), ByVal point As Point, Optional ByVal type As String = "")
        For Each obj In Colliders
            If obj.Bounds.IntersectsWith(New Rectangle(point, New Size(1, 1))) = True Then
                If type = "Collision" Then
                    Return obj
                Else
                    Return obj.Bounds.IntersectsWith(New Rectangle(point, New Size(1, 1)))
                End If
                Exit For
            End If
        Next
    End Function

    Private Function raycast(ByVal point As Point, ByVal dir As Integer, ByRef col() As PictureBox, Optional ByVal Type As String = "")
        Dim dist As Integer = 0
        dist = 0
        While collision_point(col, New Point(point.X + lengthdir_x(dist, dir), point.Y + lengthdir_y(dist, dir))) = False And dist < 1000
            dist += 1
        End While
        If Type = "Collision" Then
            If collision_point(col, New Point(point.X + lengthdir_x(dist, dir), point.Y + lengthdir_y(dist, dir))) <> Nothing Then
                Return collision_point(col, New Point(point.X + lengthdir_x(dist, dir), point.Y + lengthdir_y(dist, dir)), "Collision")
            Else
                Return "Nothing"
            End If
        ElseIf Type = "Point" Then
            dist -= 1
            Return New Point(point.X + lengthdir_x(dist, dir), point.Y + lengthdir_y(dist, dir))
        Else
            dist -= 1
            Return dist
        End If
    End Function



    Public Function point_at(ByVal pos1 As Point, ByVal pos2 As Point) As Integer 'takes in an x & y to point towards the cursor from said position.
        Dim dir As Integer, rad As Double
        'd stands for delta.         
        Dim dX As Integer = pos2.X - pos1.X
        Dim dY As Integer = pos2.Y - pos1.Y
        rad = Math.Atan2(dY, dX)
        dir = rad * (180 / Math.PI)
        Return dir
    End Function

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

    Private Sub Game_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        Player.Tag = ""
    End Sub


    Private Sub Game_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Randomize()

        'Constants
        Spawnlocations(0) = New Point(xMapSize / 2 - 16, 0)
        Spawnlocations(1) = New Point(0, yMapSize / 2 - 16)
        Spawnlocations(2) = New Point(xMapSize, yMapSize / 2 - 16)
        Spawnlocations(3) = New Point(xMapSize / 2 - 16, yMapSize)

        Const TileSize As Integer = 32

        'Initializing form
        Me.ClientSize = New Size(xMapSize, yMapSize)
        Me.Name = "The Quarry"
        Me.BackgroundImage = My.Resources.background

        'Create Objects
        'Player
        With Player
            PStats.xmove = 0
            PStats.ymove = 0
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

        'Enemy

        Controls.Add(Boss)

        SpawnBoss()


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

    Private Sub Game_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        Dim mpos As Point = PointToClient(MousePosition)
        Dim dir As Integer = point_at(Player.Location, mpos)
        Dim playerLocation As Point = New Point(Player.Location.X + 16, Player.Location.Y + 16)
        Dim bossArray(20) As PictureBox
        If CHealth > 0 And bossNum < 6 Then
            For i = 0 To 19
                bossArray(i) = Colliders(i)
            Next
            bossArray(20) = Boss

            Dim pen As New Pen(Brushes.White)
            pen.Width = 3.0F
            Dim hitLocation As Point = raycast(Player.Location, dir, bossArray, "Point")
            If e.Button = MouseButtons.Left Then
                If raycast(Player.Location, dir, bossArray, "Collision") Is Boss Then
                    Boss.Tag -= 50
                End If
            End If
        End If
    End Sub

End Class

Public Class Chars

    Public Structure Player
        Public Const Speed As Integer = 6
        Public Const MaxHealth As Integer = 100

        Public xmove As Integer
        Public ymove As Integer
    End Structure

    'RED
    Public Structure RedStats
        Public Const Speed As Integer = 3
        Public Const MaxHealth As Integer = 250
        Public Const Scale As Integer = 96
        Public Const Damage As Integer = 10
        'AI
        Public Sub RedAI(ByRef Red As PictureBox, ByRef target As PictureBox)
            Dim xmove As Integer, ymove As Integer
            If target.Location.X - Red.Location.X - RedStats.Scale / 2 + target.Size.Width / 2 < 0 Then
                xmove = -RedStats.Speed
            ElseIf target.Location.X - Red.Location.X - RedStats.Scale / 2 + target.Size.Width / 2 > 0 Then
                xmove = RedStats.Speed
            End If
            If target.Location.Y - Red.Location.Y - RedStats.Scale / 2 + target.Size.Height / 2 < 0 Then
                ymove = -RedStats.Speed
            ElseIf target.Location.Y - Red.Location.Y - RedStats.Scale / 2 + target.Size.Height / 2 > 0 Then
                ymove = RedStats.Speed
            End If
            Red.Location = New Point(Red.Location.X + xmove, Red.Location.Y + ymove)
        End Sub
    End Structure

    'GREEN
    Public Structure GreenStats
        Public Const Speed As Integer = 5
        Public Const MaxHealth As Integer = 150
        Public Const Scale As Integer = 64
        Public Const Damage As Integer = 7
        'AI
        Public Sub GreenAI(ByRef Green As PictureBox, ByRef target As PictureBox)
            If target.Location.X - Green.Location.X - GreenStats.Scale / 2 + target.Size.Width / 2 < 0 Then
                Green.Left -= GreenStats.Speed
            ElseIf target.Location.X - Green.Location.X - GreenStats.Scale / 2 + target.Size.Width / 2 > 0 Then
                Green.Left += GreenStats.Speed
            End If
            If target.Location.Y - Green.Location.Y - GreenStats.Scale / 2 + target.Size.Height / 2 < 0 Then
                Green.Top -= GreenStats.Speed
            ElseIf target.Location.Y - Green.Location.Y - GreenStats.Scale / 2 + target.Size.Height / 2 > 0 Then
                Green.Top += GreenStats.Speed
            End If
        End Sub
    End Structure

    'BLUE
    Public Structure BlueStats
        Public Const Speed As Integer = 3
        Public Const MaxHealth As Integer = 250
        Public Const Scale As Integer = 96
        Public Const Damage As Integer = 20
        'AI
        Public Sub BlueAI(ByRef Blue As PictureBox, ByRef target As PictureBox)
            If target.Location.X - Blue.Location.X - BlueStats.Scale / 2 + target.Size.Width / 2 < 0 Then
                Blue.Left -= BlueStats.Speed
            ElseIf target.Location.X - Blue.Location.X - BlueStats.Scale / 2 + target.Size.Width / 2 > 0 Then
                Blue.Left += BlueStats.Speed
            End If
            If target.Location.Y - Blue.Location.Y - BlueStats.Scale / 2 + target.Size.Height / 2 < 0 Then
                Blue.Top -= BlueStats.Speed
            ElseIf target.Location.Y - Blue.Location.Y - BlueStats.Scale / 2 + target.Size.Height / 2 > 0 Then
                Blue.Top += BlueStats.Speed
            End If
        End Sub
    End Structure

    'PURPLE
    Public Structure PurpleStats
        Public Const Speed As Integer = 2
        Public Const MaxHealth As Integer = 350
        Public Const Scale As Integer = 128
        Public Const Damage As Integer = 35
        'AI
        Public Sub PurpleAI(ByRef Purple As PictureBox, ByRef target As PictureBox)
            If target.Location.X - Purple.Location.X - PurpleStats.Scale / 2 + target.Size.Width / 2 < 0 Then
                Purple.Left -= PurpleStats.Speed
            ElseIf target.Location.X - Purple.Location.X - PurpleStats.Scale / 2 + target.Size.Width / 2 > 0 Then
                Purple.Left += PurpleStats.Speed
            End If
            If target.Location.Y - Purple.Location.Y - PurpleStats.Scale / 2 + target.Size.Height / 2 < 0 Then
                Purple.Top -= PurpleStats.Speed
            ElseIf target.Location.Y - Purple.Location.Y - PurpleStats.Scale / 2 + target.Size.Height / 2 > 0 Then
                Purple.Top += PurpleStats.Speed
            End If
        End Sub
    End Structure

    'TRANSPARENT
    Public Structure TransStats
        Public Const Speed As Integer = 5
        Public Const MaxHealth As Integer = 100
        Public Const Scale As Integer = 48
        Public Const Damage As Integer = 5
        'AI
        Public Sub TransAI(ByRef Trans As PictureBox, ByRef target As PictureBox)
            If target.Location.X - Trans.Location.X - TransStats.Scale / 2 + target.Size.Width / 2 < 0 Then
                Trans.Left -= TransStats.Speed
            ElseIf target.Location.X - Trans.Location.X - TransStats.Scale / 2 + target.Size.Width / 2 > 0 Then
                Trans.Left += TransStats.Speed
            End If
            If target.Location.Y - Trans.Location.Y - TransStats.Scale / 2 + target.Size.Height / 2 < 0 Then
                Trans.Top -= TransStats.Speed
            ElseIf target.Location.Y - Trans.Location.Y - TransStats.Scale / 2 + target.Size.Height / 2 > 0 Then
                Trans.Top += TransStats.Speed
            End If
        End Sub
    End Structure

End Class

