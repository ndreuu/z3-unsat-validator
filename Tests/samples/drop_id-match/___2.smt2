;(set-logic UFDT)

(declare-datatypes ((Nat_0 0)) (((S_0 (projS_0 Nat_0)) (Z_0))))

(declare-datatypes ((list_0 0)) (((nil_0) (cons_0 (head_0 Nat_0) (tail_0 list_0)))))

(declare-fun drop_0 (list_0 Nat_0 list_0) Bool)

(declare-fun query_0 (Nat_0 list_0 list_0 list_0 list_0) Bool)

(assert (forall ((A_1 list_0) (B_1 Nat_0) (C_1 Nat_0) (D_1 Nat_0) (E_1 list_0) (F_0 list_0))
	(=> (and (drop_0 F_0 C_1 E_1) (= A_1 (cons_0 D_1 E_1)) (= B_1 (S_0 C_1))) (drop_0 F_0 B_1 A_1))))

(assert (drop_0 nil_0 Z_0 nil_0))

(assert (not (drop_0 nil_0 (S_0 Z_0) (cons_0 Z_0 nil_0))))

(check-sat)

