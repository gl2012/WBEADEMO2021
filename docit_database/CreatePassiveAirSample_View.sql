Create view dbo.PassAirSample_Views
as

 select c.chain_of_custody_id,s.sample_id,s.wbea_id,s.media_serial_number,s.date_recieved_from_lab, c.date_deployed,c.date_sample_retrieved,c.date_shipped_to_lab,
 (select first_name+' '+last_name from users where user_id=c.created_by) as Create_by,(select first_name+' '+last_name from users where user_id=c.deployed_by) as Deployed_by,
 (select first_name+' '+last_name from users where user_id=c.retrieved_by) as Retrieved_by ,(select first_name+' '+last_name from users where user_id=c.shipped_by) as Shipped_by ,(select name from Locations where location_id=c.location_id) as location_name 
 from  ChainOfCustodys c left join ChainOfCustodys_Samples cs on c.chain_of_custody_id=cs.chain_of_custody_id join samples s on  s.sample_id=cs.sample_id  
 where c.sample_type_id=9 