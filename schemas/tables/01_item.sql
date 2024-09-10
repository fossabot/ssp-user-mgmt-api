-- Table: public.item

-- DROP TABLE IF EXISTS public.item;

CREATE TABLE IF NOT EXISTS public.item
(
    id bigint NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 9223372036854775807 CACHE 1 ),
    guid uuid NOT NULL DEFAULT gen_random_uuid(),
    verified boolean NOT NULL DEFAULT false,
    CONSTRAINT pk_item_id PRIMARY KEY (id),
    CONSTRAINT uq_item_guid UNIQUE (guid)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.item
    OWNER to postgres;
