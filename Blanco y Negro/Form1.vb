Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        setBackgroundImageOnGrayScale()

    End Sub

    Private Sub setBackgroundImageOnGrayScale()
        Dim dlg As OpenFileDialog = New OpenFileDialog()
        dlg.Filter = "Image files (*.BMP, *.JPG, *.GIF, *.PNG)|*.bmp;*.jpg;*.gif;*.png"
        If dlg.ShowDialog() = DialogResult.OK Then
            Dim img As Image = Image.FromFile(dlg.FileName)
            Dim bm As Bitmap = New Bitmap(img.Width, img.Height)
            Dim g As Graphics = Graphics.FromImage(bm)
            'Dim cm As ColorMatrix = New ColorMatrix(New Single()() _
            '     {New Single() {0.5, 0.5, 0.5, 0, 0}, _
            '    New Single() {0.5, 0.5, 0.5, 0, 0}, _
            '    New Single() {0.5, 0.5, 0.5, 0, 0}, _
            '    New Single() {0, 0, 0, 1, 0}, _
            '    New Single() {0, 0, 0, 0, 1}})


            'Gilles Khouzams colour corrected grayscale shear

            Dim cm As ColorMatrix = New ColorMatrix(New Single()() _
                 {New Single() {0.3, 0.3, 0.3, 0, 0},
                New Single() {0.59, 0.59, 0.59, 0, 0},
                New Single() {0.11, 0.11, 0.11, 0, 0},
                New Single() {0, 0, 0, 1, 0},
                New Single() {0, 0, 0, 0, 1}})


            Dim attributes As ImageAttributes = New ImageAttributes()
            attributes.SetColorMatrix(cm)
            g.DrawImage(img, New Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attributes)
            g.Dispose()
            Me.BackgroundImage = bm
        End If
    End Sub

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles MyBase.Paint

        'Create a bitmap in a fixed ratio to the original drawing area.
        Dim bm As New Bitmap(CInt(Me.ClientSize.Width / 5), CInt(Me.ClientSize.Height / 5))

        'Create a GraphicsPath object. 
        Dim pth As New GraphicsPath()

        'Add the string in the chosen style. 
        pth.AddString("Edgar Lopez", New FontFamily("Verdana"), CInt(FontStyle.Regular), 100, New Point(20, 20), StringFormat.GenericTypographic)

        'Get the graphics object for the image. 
        Dim g As Graphics = Graphics.FromImage(bm)

        'Create a matrix that shrinks the drawing output by the fixed ratio. 
        Dim mx As New Matrix(1.0F / 5, 0, 0, 1.0F / 5, -(1.0F / 5), -(1.0F / 5))

        'Choose an appropriate smoothing mode for the halo. 
        g.SmoothingMode = SmoothingMode.AntiAlias

        'Transform the graphics object so that the same half may be used for both halo and text output. 
        g.Transform = mx

        'Using a suitable pen...
        Dim p As New Pen(Color.Red, 3)

        'Draw around the outline of the path
        g.DrawPath(p, pth)

        'and then fill in for good measure. 
        g.FillPath(Brushes.Yellow, pth)

        'We no longer need this graphics object
        g.Dispose()

        'this just shifts the effect a little bit so that the edge isn't cut off in the demonstration
        e.Graphics.Transform = New Matrix(1, 0, 0, 1, 50, 50)

        'setup the smoothing mode for path drawing
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

        'and the interpolation mode for the expansion of the halo bitmap
        e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic

        'expand the halo making the edges nice and fuzzy. 
        e.Graphics.DrawImage(bm, ClientRectangle, 0, 0, bm.Width, bm.Height, GraphicsUnit.Pixel)

        'Redraw the original text
        e.Graphics.FillPath(Brushes.Beige, pth)

        'and you're done. 
        pth.Dispose()
    End Sub 'Form1_Paint
End Class
