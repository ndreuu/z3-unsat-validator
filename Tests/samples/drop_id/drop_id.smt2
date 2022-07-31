(set-logic HORN)
(set-option :produce-proofs true)

(declare-datatypes ((Nat 0)) (((S (projS Nat)) (Z))))
(declare-datatypes ((list 0)) (((nil) (cons (head Nat) (tail list)))))

(declare-fun drop (list Nat list) Bool)
(assert (forall ((y list)) (drop y Z y)))
(assert (forall ((x Nat) (y1 Nat) (y list) (r list))
    (=> (drop r x y)
        (drop r (S x) (cons y1 y)))))
(assert (forall ((x Nat)) (drop nil (S x) nil)))

(assert (forall ((n Nat) (l1 list) (l2 list) (r2 list) (r1 list))
    (=> (and (drop l2 n l1)
            (drop r2 n l2)
            (drop r1 n l1))
        (= r1 r2))))
(check-sat)
(get-proof)




(set-logic HORN)

(declare-fun query!0 (Nat list list list list) Bool)
(proof
(mp 
((_ hyper-res 0 0 0 1 0 2 0 3)
	(asserted (forall ((A Nat) (B list) (C list) (D list) (E list) )(=> (and (drop D A C) (drop E A B) (drop C A B) (not (= E D))) (query!0 A B C D E)))) 
	((_ hyper-res 0 0 0 1) 
		(asserted (forall ((A list) (B Nat) (C Nat) (D Nat) (E list) (F list) )(=> (and (drop F C E) (= A (cons D E)) (= B (S C))) (drop F B A)))) 
		((_ hyper-res 0 0) 
			(asserted (forall ((A list) )(drop A Z A))) 
			(drop nil Z nil)) 
	(drop nil (S Z) (cons Z nil))) 
	((_ hyper-res 0 0 0 1) 
		(asserted (forall ((A list) (B Nat) (C Nat) (D Nat) (E list) (F list) )(=> (and (drop F C E) (= A (cons D E)) (= B (S C))) (drop F B A)))) 
		((_ hyper-res 0 0) 
			(asserted (forall ((A list) )(drop A Z A))) 
			(drop (cons Z nil) Z (cons Z nil))) 
	(drop (cons Z nil) (S Z) (cons Z (cons Z nil)))) 
	((_ hyper-res 0 0 0 1) 
		(asserted (forall ((A list) (B Nat) (C Nat) (D Nat) (E list) (F list) )(=> (and (drop F C E) (= A (cons D E)) (= B (S C))) (drop F B A)))) 
		((_ hyper-res 0 0) 
			(asserted (forall ((A list) )(drop A Z A))) 
			(drop (cons Z nil) Z (cons Z nil))) 
	(drop (cons Z nil) (S Z) (cons Z (cons Z nil)))) 
(query!0 (S Z) (cons Z (cons Z nil)) (cons Z nil) nil (cons Z nil)))
 
(asserted (=> (query!0 (S Z) (cons Z (cons Z nil)) (cons Z nil) nil (cons Z nil)) false)) 

false))


;(declare-fun drop (list Nat list) Bool)
;(assert (forall ((y list)) (drop y Z y)))
;(assert (forall ((x Nat) (y1 Nat) (y list) (r list))
;    (=> (drop r x y)
;        (drop r (S x) (cons y1 y)))))
;(assert (forall ((x Nat)) (drop nil (S x) nil)))
;
;(assert (forall ((n Nat) (l1 list) (l2 list) (r2 list) (r1 list))
;    (=> (and (drop l2 n l1)
;            (drop r2 n l2)
;            (drop r1 n l1))
;        (= r1 r2))))
;(check-sat)
;(get-proof)


