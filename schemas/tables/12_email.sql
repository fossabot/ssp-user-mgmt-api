-- Table: public.email

-- DROP TABLE IF EXISTS public.email;

CREATE TABLE IF NOT EXISTS public.email
(
    item_id bigint NOT NULL,
    address character varying(255) COLLATE pg_catalog."default" NOT NULL,
    version integer NOT NULL,
    description text COLLATE pg_catalog."default",
    verified boolean NOT NULL DEFAULT false,
    CONSTRAINT uq_email_address UNIQUE (address),
    CONSTRAINT fk_item_id FOREIGN KEY (item_id)
        REFERENCES public.item (id) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.email
    OWNER to postgres;
