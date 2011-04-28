Imports System
Imports System.Collections.Generic
Imports Facebook

Namespace $rootnamespace$.Samples.Facebook.VB

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
                Dim fb As New FacebookClient()

                Dim result = DirectCast(fb.Get("/4"), IDictionary(Of String, Object))

                Dim id = DirectCast(result("id"), String)
                Dim name = DirectCast(result("name"), String)
                Dim firstName = DirectCast(result("first_name"), String)
                Dim lastName = DirectCast(result("last_name"), String)

                Console.WriteLine("Id: {0}", id)
                Console.WriteLine("Name: {0}", name)
                Console.WriteLine("First Name: {0}", firstName)
                Console.WriteLine("Last Name: {0}", lastName)
                Console.WriteLine()

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())
            Catch ex As FacebookApiException
                ' Note: make sure to handle this excepion.
                Throw
            End Try
        End Sub

        Public Shared Sub GetSampleWithAccessToken(accessToken As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = DirectCast(fb.Get("/me"), IDictionary(Of String, Object))

                Dim id = DirectCast(result("id"), String)
                Dim name = DirectCast(result("name"), String)
                Dim firstName = DirectCast(result("first_name"), String)
                Dim lastName = DirectCast(result("last_name"), String)

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

                Dim result = DirectCast(fb.Post("/me/feed", New Dictionary(Of String, Object)() From {{"message", message}}), IDictionary(Of String, Object))
                Dim postId = DirectCast(result("id"), String)

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

                Dim result = DirectCast(fb.Post("me/photos", New Dictionary(Of String, Object)() From { _
                                                                    {"source", mediaObject}, _
                                                                    {"message", "photo"} _
                                                                }), IDictionary(Of String, Object))

                Dim postId = DirectCast(result("id"), String)

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