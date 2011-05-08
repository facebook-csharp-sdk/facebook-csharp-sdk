Imports System
Imports System.Dynamic
Imports Facebook

Namespace $rootnamespace$.Samples.Facebook.Dynamic.VB

    Public NotInheritable Class LegacyRestApiSamples

        Private Sub New()
        End Sub

        Public Shared Sub RunSamples(accessToken As String)
            GetSample(accessToken)

            Dim postId = PostToMyWall(accessToken, "message posted from Facebook C# SDK sample using rest api")

            Console.WriteLine()
            Console.WriteLine("Goto www.facebook.com and check if the message was posted in the wall. Then press any key to continue")
            Console.ReadKey()

            DeletePost(accessToken, postId)

            Console.WriteLine()
            Console.WriteLine("Goto www.facebook.com and check if the message was deleted in the wall. Then press any key to continue")
            Console.ReadKey()
        End Sub


        Public Shared Sub GetSample(accessToken As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim parameters As Object = New ExpandoObject()
                parameters.method = "pages.isFan"
                parameters.page_id = "162171137156411" ' id of http://www.facebook.com/csharpsdk official page
                Dim isFan = fb.Get(parameters)

                If isFan Then
                    Console.WriteLine("You are a fan of http://www.facebook.com/csharpsdk")
                Else
                    Console.WriteLine("You are not a fan of http://www.facebook.com/csharpsdk")
                End If
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Function PostToMyWall(accessToken As String, message As String) As String
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim parameters As Object = New ExpandoObject()
                parameters.method = "stream.publish"
                parameters.message = message

                Dim result = fb.Post(parameters)
                Dim postId = result

                Console.WriteLine("Post Id: {0}", postId)

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())

                Return postId
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Function

        Public Shared Function DeletePost(accessToken As String, postId As String) As String
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim parameters As Object = New ExpandoObject()
                parameters.method = "stream.remove"
                parameters.post_id = postId

                Dim result = fb.Post(parameters)

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())

                Return postId
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Function

        Public Shared Function UploadPhoto(accessToken As String, filePath As String) As String
            ' sample usage: UploadPhoto(accessToken, "C:\Users\Public\Pictures\Sample Pictures\Penguins.jpg")

            Dim mediaObject As New FacebookMediaObject()
            With mediaObject
                mediaObject.FileName = System.IO.Path.GetFileName(filePath)
                mediaObject.ContentType = "image/jpeg"
            End With

            mediaObject.SetValue(System.IO.File.ReadAllBytes(filePath))

            Try
                Dim fb = New FacebookClient(accessToken)

                Dim parameters As Object = New ExpandoObject()
                parameters.method = "facebook.photos.upload"
                parameters.caption = "photo upload using rest api"
                parameters.source = mediaObject

                Dim result = fb.Post(parameters)

                Dim pictureId = result.pid

                Console.WriteLine("Picture Id: {0}", pictureId)

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())

                Return pictureId
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Function

    End Class

End Namespace