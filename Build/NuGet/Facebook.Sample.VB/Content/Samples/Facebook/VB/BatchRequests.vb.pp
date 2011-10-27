Imports System
Imports System.Collections.Generic
Imports Facebook

Namespace $rootnamespace$.Samples.Facebook.VB

    Public NotInheritable Class BatchRequestsSamples

        Private Sub New()
        End Sub

        Public Shared Sub RunSamples(accessToken As String)
            BatchRequest(accessToken)
            BatchRequestWithFql(accessToken)
            BatchRequestWithFqlMultiQuery(accessToken)
        End Sub

        Public Shared Sub BatchRequest(accessToken As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = DirectCast(fb.Batch( _
                                            New FacebookBatchParameter("me"), _
                                            New FacebookBatchParameter(HttpMethod.Get, "me/friends", New With {Key .limit = 10})), IList(Of Object))

                Dim result0 = result(0)
                Dim result1 = result(1)

                ' Note: Always check first if each result set is an exeption.

                If TypeOf result0 Is Exception Then
                    Dim ex = DirectCast(result0, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Dim [me] = DirectCast(result0, IDictionary(Of String, Object))
                    Dim name = DirectCast([me]("name"), String)

                    Console.WriteLine("Hi {0}", name)
                End If

                Console.WriteLine()

                If TypeOf result1 Is Exception Then
                    Dim ex = DirectCast(result1, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Dim friends = DirectCast(DirectCast(result1, IDictionary(Of String, Object))("data"), IList(Of Object))

                    Console.WriteLine("Some of your friends: ")

                    For Each [friend] As IDictionary(Of String, Object) In friends
                        Console.WriteLine([friend]("name"))
                    Next
                End If
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Sub BatchRequestWithFql(accessToken As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = DirectCast(fb.Batch(
                    New FacebookBatchParameter("/4"), _
                    New FacebookBatchParameter().Query("SELECT name FROM user WHERE uid=me()")), IList(Of Object))

                Dim result0 = result(0)
                Dim result1 = result(1)

                ' Note: Always check first if each result set is an exeption.

                If TypeOf result0 Is Exception Then
                    Dim ex = DirectCast(result0, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Console.WriteLine("Batch Result 0: {0}", result0)
                End If

                Console.WriteLine()

                If TypeOf result1 Is Exception Then
                    Dim ex = DirectCast(result1, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Dim fqlResult = DirectCast((DirectCast(result1, IDictionary(Of String, Object)))("data"), IList(Of Object))

                    Dim fqlResult1 = DirectCast(fqlResult(0), IDictionary(Of String, Object))
                    Console.WriteLine("Hi {0}", fqlResult1("name"))
                End If
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Sub BatchRequestWithFqlMultiQuery(accessToken As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = DirectCast(fb.Batch(
                    New FacebookBatchParameter("/4"), _
                    New FacebookBatchParameter().Query("SELECT first_name FROM user WHERE uid=me()", "SELECT last_name FROM user WHERE uid=me()")), IList(Of Object))

                Dim result0 = result(0)
                Dim result1 = result(1)

                ' Note: Always check first if each result set is an exeption.

                If TypeOf result0 Is Exception Then
                    Dim ex = DirectCast(result0, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Console.WriteLine("Batch Result 0: {0}", result0)
                End If

                Console.WriteLine()

                If TypeOf result1 Is Exception Then
                    Dim ex = DirectCast(result1, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Dim fqlResult = DirectCast((DirectCast(result1, IDictionary(Of String, Object)))("data"), IList(Of Object))

                    Dim fqlResultSet0 = DirectCast(fqlResult(0), IDictionary(Of String, Object))("fql_result_set")
                    Console.Write(fqlResultSet0)
                    Console.WriteLine()
                    Dim fqlResultSet1 = DirectCast(fqlResult(1), IDictionary(Of String, Object))("fql_result_set")
                    Console.WriteLine(fqlResultSet1)
                End If
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

    End Class

End Namespace