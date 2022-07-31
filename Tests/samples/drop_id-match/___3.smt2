;(set-logic UFDT)

(declare-datatypes ((Nat_0 0)) (((S_0 (projS_0 Nat_0)) (Z_0))))

(declare-datatypes ((list_0 0)) (((nil_0) (cons_0 (head_0 Nat_0) (tail_0 list_0)))))

(declare-fun drop_0 (list_0 Nat_0 list_0) Bool)

(declare-fun query_0 (Nat_0 list_0 list_0 list_0 list_0) Bool)

(assert (forall ((A_2 list_0))
	(drop_0 A_2 Z_0 A_2)))

(assert (not (drop_0 nil_0 Z_0 nil_0)))

(check-sat)

