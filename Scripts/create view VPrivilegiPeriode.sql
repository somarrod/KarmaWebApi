
CREATE VIEW VPrivilegiPeriode AS 
SELECT	ag.idAlumneEnGrup as IdAlumneEnGrup, pd.idPeriode as IdPeriode, pv.idPrivilegi as IdPrivilegi
FROM	Privilegi pv, Periode pd, AlumneEnGrup ag, ConfiguracioKarma cg
WHERE	pv.Nivell = cg.NivellPrivilegis AND 
		cg.KarmaMinim <= ag.PuntuacioTotal AND 
		cg.KarmaMaxim >= ag.PuntuacioTotal AND
		pv.EsIndividualGrup = 'I'
UNION ALL
SELECT	ag.idAlumneEnGrup as IdAlumneEnGrup, pd.idPeriode as IdPeriode, pv.idPrivilegi as IdPrivilegi
FROM	Privilegi pv, Periode pd, AlumneEnGrup ag, ConfiguracioKarma cg, Grup g
WHERE	pv.Nivell = cg.NivellPrivilegis AND 
		cg.ColorNivell = g.KarmaBase AND 
		g.idgrup = ag.idgrup AND 
		pv.EsIndividualGrup = 'G'
GO