-- Table: public.algorithms

-- DROP TABLE IF EXISTS public.algorithms;

CREATE TABLE IF NOT EXISTS public.algorithms
(
    name text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT pk_algorithms PRIMARY KEY (name)
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.algorithms
    OWNER to postgres;