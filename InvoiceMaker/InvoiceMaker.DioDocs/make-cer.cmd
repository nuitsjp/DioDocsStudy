makecert -n "CN=DioDocs,C=JP" -a sha256 -b 01/01/2000 -e 01/01/2100 -r -sv diodocs.pvk diodocs.cer
pvk2pfx -pvk diodocs.pvk -spc diodocs.cer -pfx diodocs.pfx -f -pi diodocs

certmgr -add -c diodocs.cer -s root 
certmgr -add -c diodocs.cer -s trustedpublisher