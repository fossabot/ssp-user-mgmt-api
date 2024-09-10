-- View: public.list

-- DROP VIEW public.list;

CREATE OR REPLACE VIEW public.list
 AS
 SELECT item.id,
    item.guid,
    name.display
   FROM item
     LEFT JOIN name ON item.id = name.item_id
  ORDER BY item.id;

ALTER TABLE public.list
    OWNER TO postgres;
