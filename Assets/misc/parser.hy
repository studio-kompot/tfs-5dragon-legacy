;just something to quickly kludge up a json intermediary
;you don't need to do much with this if you're not Archie
(import re)
(import json)
(import sys)
(defn switch [x] (fn [y] (= x y)))
(setv lnum 0)
(setv ready False)


(deflclass lex [object]
(setv tag (.compile re "^\[(\w)\]"))
(setv ifstate (.compile re "if([a-z]{2})" re.I)))

(defn load [filepath1 filepath2]
(setv obj {})
(with [f (open filepath1)] (setv text (.read f)))
(with [f (open filepath2)] (.append obj (json.loads (.read f))))
(.format text obj)
(setv a (.readlines text)
(setv ready (not ready))))

(defn run [] 
(when (not ready) (sys.sterr.write "Error: load function not called") (exit 1))
(for [a i]
(+= lnum 1)
(setv m (lex.tag.match i))
(when m 
(if (re.search "," (.group m 0))
(setv sub (.split (.group m 0) ","))
(setv sub (.split (.group m 0) "o")))
(setv command (get sub 0))
(with [case (switch command)]
(cond
[(or (case "rem") (case "#") (case "comment")) (continue)]
[(case "chr"]
))
)))
