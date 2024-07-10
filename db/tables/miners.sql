-- Table: public.miners

-- DROP TABLE IF EXISTS public.miners;

CREATE TABLE IF NOT EXISTS public.miners
(
    name text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_miners PRIMARY KEY (name)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.miners
    OWNER to postgres;