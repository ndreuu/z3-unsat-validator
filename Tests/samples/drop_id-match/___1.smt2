;(set-logic UFDT)

(declare-datatypes ((Nat_0 0)) (((S_0 (projS_0 Nat_0)) (Z_0))))

(declare-datatypes ((list_0 0)) (((nil_0) (cons_0 (head_0 Nat_0) (tail_0 list_0)))))

(declare-fun drop_0 (list_0 Nat_0 list_0) Bool)

(declare-fun query_0 (Nat_0 list_0 list_0 list_0 list_0) Bool)

(assert (forall ((A_0 Nat_0) (B_0 list_0) (C_0 list_0) (D_0 list_0) (E_0 list_0))
	(=> (and (drop_0 D_0 A_0 C_0) (drop_0 E_0 A_0 B_0) (drop_0 C_0 A_0 B_0) (not (= E_0 D_0))) (query_0 A_0 B_0 C_0 D_0 E_0))))

(assert (drop_0 nil_0 (S_0 Z_0) (cons_0 Z_0 nil_0)))

(assert (drop_0 (cons_0 Z_0 nil_0) (S_0 Z_0) (cons_0 Z_0 (cons_0 Z_0 nil_0))))

(assert (drop_0 (cons_0 Z_0 nil_0) (S_0 Z_0) (cons_0 Z_0 (cons_0 Z_0 nil_0))))

(assert (not (query_0 (S_0 Z_0) (cons_0 Z_0 (cons_0 Z_0 nil_0)) (cons_0 Z_0 nil_0) nil_0 (cons_0 Z_0 nil_0))))

(check-sat)

