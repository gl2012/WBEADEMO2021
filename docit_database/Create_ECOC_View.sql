
Create view dbo.ECOCData_Views
as
SELECT c.chain_of_custody_id AS ECOCData_View_Id, l.name AS SITE, format(c.date_actual_sample_start, 'MM/dd/yyyy') AS DATE, 2.5 AS Size, sc.name, c.date_actual_sample_start AS Date_actual, c.sample_volume AS TVOC, '' AS TVOU, S.wbea_id AS WEBA_ID, S.lab_sample_id AS TID, 
             N.body AS [WBEA NOTES], sc.schedule_id
FROM   dbo.ChainOfCustodys AS c RIGHT OUTER JOIN
             dbo.Schedules AS sc ON c.schedule_id = sc.schedule_id LEFT OUTER JOIN
             dbo.Locations AS l ON c.location_id = l.location_id LEFT OUTER JOIN
             dbo.ChainOfCustodys_Samples AS CS LEFT OUTER JOIN
             dbo.Samples AS S ON CS.sample_id = S.sample_id ON c.chain_of_custody_id = CS.chain_of_custody_id LEFT OUTER JOIN
             dbo.Notes_ChainOfCustodys AS NC INNER JOIN
             dbo.Notes AS N ON NC.note_id = N.note_id ON c.chain_of_custody_id = NC.chain_of_custody_id
WHERE (c.sample_type_id = 1) AND (CHARINDEX('EC/OC', sc.name) > 0)
