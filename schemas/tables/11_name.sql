-- Table: public.name

-- DROP TABLE IF EXISTS public.name;

CREATE TABLE IF NOT EXISTS public.name
(
    item_id bigint NOT NULL,
    version integer NOT NULL DEFAULT 1,
    first character varying(64) COLLATE pg_catalog."default",
    middle character varying(64) COLLATE pg_catalog."default",
    last character varying(64) COLLATE pg_catalog."default",
    display character varying(64) COLLATE pg_catalog."default",
    CONSTRAINT uq_name_item_id UNIQUE (item_id),
    CONSTRAINT fk_item_id FOREIGN KEY (item_id)
        REFERENCES public.item (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.name
    OWNER to postgres;
