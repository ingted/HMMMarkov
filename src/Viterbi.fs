namespace markov.core
    open System

    module Viterbi =
        let private getMaxes(T1: double[,], i: int, selector) =
            T1.[0.., i]
            |> Array.mapi selector
            |> Array.maxBy snd

        let private getMax(T1: double[,], A: double[][], i: int, j: int) =
            let selector = fun index elem -> (index, elem * A.[index].[j])
            getMaxes(T1, i - 1, selector)

        let private getMaxIndex(T1: double[,], n: int) =
            let selector = fun index elem -> (index, elem)
            getMaxes(T1, n, selector) |> fst

        let Calculate(space: int[], states: int[], probabilities: double[], observations: int[], A: double[][], B: double[][]) =
            let T1: double[,] = Array2D.create states.Length observations.Length 0.0
            let T2: int[,] = Array2D.create states.Length observations.Length 0
            let z = Array.zeroCreate observations.Length
            let x = Array.zeroCreate observations.Length
            for i in [0..states.Length - 1] do
                T1.[i, 1] <- probabilities.[i] * B.[0].[0]

            for i in [1..observations.Length - 1] do
                for j in [0..states.Length - 1] do
                    let maxes = getMax(T1, A, i, j)
                    T1.[j, i] <- B.[j].[observations.[i]] * snd maxes
                    T2.[j, i] <- fst maxes
            z.[observations.Length - 1] <- getMaxIndex(T1, observations.Length - 1)
            x.[observations.Length - 1] <- states.[z.[observations.Length - 1]]
            for i = observations.Length - 1 downto 1 do
                z.[i-1] <- T2.[z.[i], i]
                x.[i - 1] <- space.[z.[i-1]]
            x

            
