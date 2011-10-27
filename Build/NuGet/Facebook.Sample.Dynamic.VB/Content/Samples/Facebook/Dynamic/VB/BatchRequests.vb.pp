Imports System
Imports Facebook

Namespace $rootnamespace$.Samples.Facebook.Dynamic.VB

    Public NotInheritable Class BatchRequestSamples

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

                Dim result = fb.Batch(
                    New FacebookBatchParameter("me"), _
                    New FacebookBatchParameter(HttpMethod.[Get], "me/friends", New With {Key .limit = 10}))

                Dim result0 = result(0)
                Dim result1 = result(1)

                ' Note: Always check first if each result set is an exeption.

                If TypeOf result0 Is Exception Then
                    Dim ex = DirectCast(result0, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Dim [me] = result0
                    Dim name = [me].name

                    Console.WriteLine("Hi {0}", name)
                End If

                Console.WriteLine()

                If TypeOf result1 Is Exception Then
                    Dim ex = DirectCast(result1, Exception)
                    ' Note: make sure to handle this exception.
                    Throw ex
                Else
                    Dim friends = result1.data

                    Console.WriteLine("Some of your friends: ")

                    For Each [friend] As Object In friends
                        Console.WriteLine([friend].name)
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

                Dim result = fb.Batch(
                    New FacebookBatchParameter("/4"),
                    New FacebookBatchParameter().Query("SELECT name FROM user WHERE uid=me()"))

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
                    Dim fqlResult = result1.data

                    Dim fqlResult1 = fqlResult(0)
                    Console.WriteLine("Hi {0}", fqlResult1.name)
                End If
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

        Public Shared Sub BatchRequestWithFqlMultiQuery(accessToken As String)
            Try
                Dim fb = New FacebookClient(accessToken)

                Dim result = fb.Batch(
                    New FacebookBatchParameter("/4"),
                    New FacebookBatchParameter().Query("SELECT first_name FROM user WHERE uid=me()", "SELECT last_name FROM user WHERE uid=me()"))

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
                    Dim fqlResult = result1.data

                    Dim fqlResultSet0 = fqlResult(0).fql_result_set
                    Console.Write(fqlResultSet0)
                    Console.WriteLine()
                    Dim fqlResultSet1 = fqlResult(1).fql_result_set
                    Console.WriteLine(fqlResultSet1)
                End If
            Catch ex As FacebookApiException
                ' Note: make sure to handle this exception.
                Throw
            End Try
        End Sub

    End Class

End Namespace