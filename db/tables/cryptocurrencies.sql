-- Table: public.cryptocurrencies

-- DROP TABLE IF EXISTS public.cryptocurrencies;

CREATE TABLE IF NOT EXISTS public.cryptocurrencies
(
    id uuid NOT NULL,
    short_name text COLLATE pg_catalog."default" NOT NULL,
    full_name text COLLATE pg_catalog."default" NOT NULL,
    algorithm text COLLATE pg_catalog."default" NOT NULL,
    user_id bigint NOT NULL,
    CONSTRAINT pk_cryptocurrencies PRIMARY KEY (id),
    CONSTRAINT fk_cryptocurrencies_algorithms_algorithm FOREIGN KEY (algorithm)
        REFERENCES public.algorithms (name) MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
)

TABLESPACE pg_default;

ALTER TABLE IF EXISTS public.cryptocurrencies
    OWNER to postgres;
-- Index: ix_cryptocurrencies_algorithm

-- DROP INDEX IF EXISTS public.ix_cryptocurrencies_algorithm;

CREATE INDEX IF NOT EXISTS ix_cryptocurrencies_algorithm
    ON public.cryptocurrencies USING btree
    (algorithm COLLATE pg_catalog."default" ASC NULLS LAST)
    TABLESPACE pg_default;
-- Index: ix_cryptocurrencies_user_id

-- DROP INDEX IF EXISTS public.ix_cryptocurrencies_user_id;

CREATE INDEX IF NOT EXISTS ix_cryptocurrencies_user_id
    ON public.cryptocurrencies USING btree
    (user_id ASC NULLS LAST)
    TABLESPACE pg_default;