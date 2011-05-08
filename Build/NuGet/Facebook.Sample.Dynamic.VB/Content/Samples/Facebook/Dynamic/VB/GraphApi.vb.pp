Imports System
Imports System.Dynamic
Imports Facebook

Namespace $rootnamespace$.Samples.Facebook.Dynamic.VB

    Public NotInheritable Class GraphApiSamples

        Private Sub New()
        End Sub

        Public Shared Sub RunSamples(accessToken As String)
            GetSampleWithoutAccessToken()
            GetSampleWithAccessToken(accessToken)

            Dim postId = PostToMyWall(accessToken, "message posted from Facebook C# SDK sample using graph api")

            Console.WriteLine()
            Console.WriteLine("Goto www.facebook.com and check if the message was posted in the wall. Then press any key to continue")
            Console.ReadKey()

            Delete(accessToken, postId)

            Console.WriteLine()
            Console.WriteLine("Goto www.facebook.com and check if the message was deleted in the wall. Then press any key to continue")
            Console.ReadKey()
        End Sub

        Public Shared Sub GetSampleWithoutAccessToken()
            Try
                Dim fb = New FacebookClient()

                Dim result = fb.Get("/4")

                Dim id = result.id
                Dim name = result.name
                Dim firstName = result.first_name
                Dim lastName = result.last_name

                Console.WriteLine("Id: {0}", id)
                Console.WriteLine("Name: {0}", name)
                Console.WriteLine("First Name: {0}", firstName)
                Console.WriteLine("Last Name: {0}", lastName)
                Console.WriteLine()

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Sub GetSampleWithAccessToken(accessToken As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = fb.Get("/me")

                Dim id = result.id
                Dim name = result.name
                Dim firstName = result.first_name
                Dim lastName = result.last_name

                Console.WriteLine("Id: {0}", id)
                Console.WriteLine("Name: {0}", name)
                Console.WriteLine("First Name: {0}", firstName)
                Console.WriteLine("Last Name: {0}", lastName)
                Console.WriteLine()

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Function PostToMyWall(accessToken As String, message As String) As String
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim parameters As Object = New ExpandoObject()
                parameters.message = message

                Dim result = fb.Post("/me/feed", parameters)

                Dim postId = result.id

                Console.WriteLine("Post Id: {0}", postId)

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())

                Return postId
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try

            Return Nothing
        End Function

        Public Shared Sub Delete(accessToken As String, id As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = fb.Delete(id)

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Function UploadPictureToWall(accessToken As String, filePath As String) As String
            ' sample usage: UploadPictureToWall(accessToken, "C:\Users\Public\Pictures\Sample Pictures\Penguins.jpg")

            Dim mediaObject As New FacebookMediaObject()
            With mediaObject
                mediaObject.FileName = System.IO.Path.GetFileName(filePath)
                mediaObject.ContentType = "image/jpeg"
            End With

            mediaObject.SetValue(System.IO.File.ReadAllBytes(filePath))

            Try
                Dim fb = New FacebookClient(accessToken)

                Dim parameters As Object = New ExpandoObject()
                parameters.source = mediaObject
                parameters.message = "photo"

                Dim result = fb.Post("me/photos", parameters)

                Dim postId = result.id

                Console.WriteLine("Post Id: {0}", postId)

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())

                Return postId
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Function

    End Class

End Namespace