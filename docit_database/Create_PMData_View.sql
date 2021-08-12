create dbo.PMDATA_VIEW
as
SELECT c.chain_of_custody_id AS PMData_View_Id, l.name AS SITE, format(c.date_actual_sample_start, 'MM/dd/yyyy') AS DATE, c.date_actual_sample_start AS Date_actual, CASE WHEN c.sample_type_id = 1 THEN '2.5' WHEN c.sample_type_id = 6 THEN '10' END AS Size, 
             c.sample_volume AS TVOC, '' AS TVOU, S.wbea_id AS WEBA_ID, S.lab_sample_id AS TID, N.body AS [WBEA NOTES], sc.name AS SMemo, CASE WHEN CHARINDEX('10 A', sc.name) > 0 OR
             CHARINDEX('2.5 A', sc.name) > 0 THEN 'A' WHEN CHARINDEX('10 B', sc.name) > 0 OR
             CHARINDEX('2.5 B', sc.name) > 0 THEN 'B' ELSE 'O' END AS GroupType
FROM   dbo.ChainOfCustodys AS c RIGHT OUTER JOIN
             dbo.Schedules AS sc ON c.schedule_id = sc.schedule_id LEFT OUTER JOIN
             dbo.Locations AS l ON c.location_id = l.location_id LEFT OUTER JOIN
             dbo.ChainOfCustodys_Samples AS CS LEFT OUTER JOIN
             dbo.Samples AS S ON CS.sample_id = S.sample_id ON c.chain_of_custody_id = CS.chain_of_custody_id LEFT OUTER JOIN
             dbo.Notes_ChainOfCustodys AS NC INNER JOIN
             dbo.Notes AS N ON NC.note_id = N.note_id ON c.chain_of_custody_id = NC.chain_of_custody_id
WHERE (c.sample_type_id IN (1, 6))