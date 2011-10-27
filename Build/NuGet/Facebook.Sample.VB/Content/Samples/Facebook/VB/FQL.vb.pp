Imports System
Imports System.Collections.Generic
Imports Facebook

Namespace $rootnamespace$.Samples.Facebook.VB

    Public NotInheritable Class FQLSamples

        Private Sub New()
        End Sub

        Public Shared Sub RunSamples(accessToken As String)
            SingleQuery(accessToken)

            Console.WriteLine()

            MultiQuery(accessToken)
        End Sub

        Public Shared Sub SingleQuery(accessToken As String)
            Dim query = String.Format("SELECT uid,pic_square FROM user WHERE uid IN (SELECT uid2 FROM friend WHERE uid1={0})", "me()")

            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = DirectCast(fb.Query(query), IList(Of Object))

                For Each row In result
                    Dim r = DirectCast(row, IDictionary(Of String, Object))
                    Dim uid = DirectCast(r("uid"), Long)
                    Dim picSquare = DirectCast(r("pic_square"), String)

                    Console.WriteLine("User Id: {0}", uid)
                    Console.WriteLine("Picture Square: {0}", picSquare)
                    Console.WriteLine()
                Next

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Sub MultiQuery(accessToken As String)
            Dim query1 = "SELECT uid FROM user WHERE uid=me()"
            Dim query2 = "SELECT profile_url FROM user WHERE uid=me()"

            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = DirectCast(fb.Query(query1, query2), IList(Of Object))

                Dim result0 = DirectCast(result(0), IDictionary(Of String, Object))("fql_result_set")
                Dim result1 = DirectCast(result(1), IDictionary(Of String, Object))("fql_result_set")

                Console.WriteLine("Query 0 result: {0}", result0)
                Console.WriteLine()
                Console.WriteLine("Query 1 result: {0}", result1)
                Console.WriteLine()

                ' Note: This json result is not the orginal json string as returned by Facebook.
                Console.WriteLine("Json: {0}", result.ToString())
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

    End Class

End Namespace